using GQ.Data.Dto;
using UCare.Domain.Comunicaciones;
using UCare.Web.Controllers.Afiliados.Dto;

namespace UCare.Web.Controllers.Comunicaciones.Dto
{
    public class ComunicacionDto : Dto<Comunicacion, ComunicacionDto>
    {
        public string? Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime? FechaEnvio { get; set; }
        public bool? Enviado { get; set; } = false;
        public string? Destinos { get; set; } = "";
        public string? DetalleEnvio { get; set; }
        public string? Estado { get; set; }

        public ModificacionDto? Creado { get; set; }
        public ModificacionDto? Modificacion { get; set; }
    }
}
