using GQ.Html.Rest;
using GQ.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Web;
using UCare.Infrastructure.GoogleApi.Data;
using UCare.Shared.Domain.Locations;
using UCare.Shared.Infrastructure.Locations;

namespace UCare.Infrastructure.GoogleApi
{
    public class GoogleMapsApi : ILocationRepository
    {
        private readonly IConfiguration configuration;
        public GoogleMapsApi(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        ///https://maps.googleapis.com/maps/api/geocode/json?latlng=10,10&key=AIzaSyBY4LDhPyLP1d-XBHpb2SMTpcwPdCkENCI
        ///
        private const string URiApi = "https://maps.googleapis.com/";
        private const string UriMap = URiApi + "maps/api/";
        private const string UriGeocode = UriMap + "geocode/";

        public HttpStatusCode StatusCode { get; private set; }
        public bool IsSuccessStatusCode { get; private set; }
        public HttpRequestMessage? RequestMessage { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <returns></returns>
        public async Task<List<Location>> GetDirectionByPos(double lat, double lon)
        {
            var result = new List<Location>();

            try
            {
                var paramData = new Dictionary<string, string>();
                paramData.Add("latlng", $"{lat.ToString().Replace(",", ".")},{lon.ToString().Replace(",", ".")}");
                paramData.Add("key", configuration["Mapa:Google:Key"]);
                var data = await GetString(UriGeocode + "json", paramData);
                var googleResult = JsonConvert.DeserializeObject<GoogleMapsGeocodeRespose>(data);

                if (googleResult?.Status == "OK")
                {
                    foreach (var item in googleResult.Results)
                    {
                        result.Add(new Location
                        {
                            StreetNumber = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "street_number") ?? false)?.LongName,
                            StreetName = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "route") ?? false)?.LongName,
                            Neighborhood = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "neighborhood") ?? false)?.LongName,
                            Locality = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "locality") ?? false)?.LongName,
                            Province = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "administrative_area_level_1") ?? false)?.LongName,
                            Country = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "country") ?? false)?.LongName,
                            PostalCode = item.AddressComponents?.FirstOrDefault(x => x?.Types?.Any(y => y == "postal_code") ?? false)?.LongName,
                            FormattedAddress = item.FormattedAddress,
                            //ROOFTOP
                            //RANGE_INTERPOLATED
                            //GEOMETRIC_CENTER
                            //APPROXIMATE
                            LocationType =
                            (item?.Geometry?.LocationType == "ROOFTOP" ? LocationTypes.Level1 :
                            (item?.Geometry?.LocationType == "RANGE_INTERPOLATED" ? LocationTypes.Level2 :
                            (item?.Geometry?.LocationType == "GEOMETRIC_CENTER" ? LocationTypes.Level3 : LocationTypes.Level4)))
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Get().Error("GetDirectionByPos", ex);
            }
            return result;
        }

        public async Task<string> GetString(string url, Dictionary<string, string> postParameters = null)
        {
            if (postParameters != null)
            {
                StringBuilder postData = new StringBuilder();

                foreach (string key in postParameters.Keys)
                {
                    postData.Append(HttpUtility.UrlEncode(key) + "="
                          + HttpUtility.UrlEncode(postParameters[key]) + "&");
                }

                var uri = new Uri(url + "?" + postData);

                return await GetString(uri);
            }
            else
                return await GetString(url);
        }

        private async Task<string> GetString(Uri uri)
        {
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync(uri);
                StatusCode = response.StatusCode;
                IsSuccessStatusCode = response.IsSuccessStatusCode;
                RequestMessage = response.RequestMessage;
                var read = await response.Content.ReadAsStringAsync();
                return read;
            }
        }
    }
}