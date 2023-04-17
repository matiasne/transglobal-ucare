using Newtonsoft.Json;

namespace UCare.Shared.Domain.CodigoPaices
{
    public class CodigoPais
    {
        [JsonProperty("country")]
        public string Country { get; set; }
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
        [JsonProperty("isoCode")]
        public string IsoCode { get; set; }

    }
}
