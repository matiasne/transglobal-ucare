using GQ.Data.Dto;
using UCare.Domain.Users;

namespace UCare.Web.Controllers.Mobile.Dto
{
    public class ComunicadoDto : Dto<ComunicacionAfiliado, ComunicadoDto>
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
    }
}
