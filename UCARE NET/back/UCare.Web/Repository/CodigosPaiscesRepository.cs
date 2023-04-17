using Newtonsoft.Json;
using System.Text.Json;
using UCare.Shared.Domain.CodigoPaices;

namespace UCare.Web.Repository
{
    public class CodigosPaiscesRepository : ICodigosPaicesRepositorio
    {
        private List<CodigoPais>? list;

        public CodigosPaiscesRepository(string ContentRootPath)
        {
            if (list == null)
            {
                using var text = File.OpenText(Path.Combine(ContentRootPath, "codigos.paices.json"));

                list = JsonConvert.DeserializeObject<countries>(text.ReadToEnd()).Countries;
            }
        }

        public CodigoPais? GetCodigoPais(string valor)
        {
            return list?.FirstOrDefault(x => x.IsoCode == valor || x.CountryCode == valor || x.Country == valor);
        }

        public List<CodigoPais>? GetCodigoPaisList()
        {
            return list;
        }
    }
    public class countries
    {
        [JsonProperty("countries")]
        public List<CodigoPais>? Countries;
    }
}
