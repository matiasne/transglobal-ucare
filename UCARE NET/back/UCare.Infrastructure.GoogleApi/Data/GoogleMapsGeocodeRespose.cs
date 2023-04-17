using Newtonsoft.Json;

namespace UCare.Infrastructure.GoogleApi.Data
{
    public class GoogleMapsGeocodeRespose : GoogleMapsResponse
    {
        [JsonProperty("results")]
        public virtual List<GoogleMapsGeocodeResult> Results { get; set; }
        [JsonProperty("plus_code")]
        public virtual GoogleMapsGeocodePlusCode PlusCode { get; set; }
    }

    public class GoogleMapsGeocodeResult
    {
        [JsonProperty("address_components")]
        public virtual List<GoogleMapsGeocodeAddress>? AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public virtual string? FormattedAddress { get; set; }
        [JsonProperty("geometry")]
        public virtual GoogleMapsGeocodeGeometry? Geometry { get; set; }
        [JsonProperty("place_id")]
        public virtual string? PlaceId { get; set; }
        [JsonProperty("plus_code")]
        public virtual GoogleMapsGeocodePlusCode? PlusCode { get; set; }
        //street_address
        //premise
        //route
        //plus_code
        //neighborhood
        //locality
        //administrative_area_level_3
        //administrative_area_level_2
        //administrative_area_level_1
        //country
        //postal_code
        //political
        [JsonProperty("types")]
        public virtual List<string>? Types { get; set; }
    }

    public class GoogleMapsGeocodeAddress
    {
        [JsonProperty("long_name")]
        public virtual string? LongName { get; set; }
        [JsonProperty("short_name")]
        public virtual string? ShortName { get; set; }
        //street_number
        //route
        //neighborhood
        //locality
        //administrative_area_level_3
        //administrative_area_level_2
        //administrative_area_level_1
        //country
        //political
        [JsonProperty("types")]
        public virtual List<string>? Types { get; set; }
    }

    public class GoogleMapsGeocodePlusCode
    {
        [JsonProperty("compound_code")]
        public virtual string? CompoundCode { get; set; }
        [JsonProperty("global_code")]
        public virtual string? GlobalCode { get; set; }
    }

    public class GoogleMapsGeocodeGeometry
    {
        [JsonProperty("location")]
        public virtual GoogleMapsGeocodeLocation? Location { get; set; }
        //ROOFTOP
        //RANGE_INTERPOLATED
        //GEOMETRIC_CENTER
        //APPROXIMATE
        [JsonProperty("location_type")]
        public virtual string? LocationType { get; set; }
        [JsonProperty("viewport")]
        public virtual GoogleMapsGeocodeViewPort? Viewport { get; set; }
        [JsonProperty("bounds")]
        public virtual GoogleMapsGeocodeViewPort? Bounds { get; set; }
    }

    public class GoogleMapsGeocodeViewPort
    {
        [JsonProperty("northeast")]
        public virtual GoogleMapsGeocodeLocation? NorthEast { get; set; }
        [JsonProperty("southwest")]
        public virtual GoogleMapsGeocodeLocation? SouthWest { get; set; }
    }

    public class GoogleMapsGeocodeLocation
    {
        [JsonProperty("lat")]
        public virtual double? Lat { get; set; }
        [JsonProperty("lng")]
        public virtual double? Lng { get; set; }
    }
}
