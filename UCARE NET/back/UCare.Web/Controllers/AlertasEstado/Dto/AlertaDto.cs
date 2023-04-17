using Google.Cloud.Firestore;
using GQ.Data.Dto;
using UCare.Domain.Alertas;
using UCare.Web.Controllers.Afiliados.Dto;
using UCare.Web.Controllers.Authentication.Dto;

namespace UCare.Web.Controllers.AlertasEstado.Dto
{
    public class AlertaDto : Dto<Alerta, AlertaDto>
    {
        public virtual string Id { get; set; } = string.Empty;
        public virtual GeoPositionDto Position { get; set; } = new GeoPositionDto();
        public virtual string AfiliadoId { get; set; } = string.Empty;
        public virtual string? AfiliadoNombreCompleto { get; set; }
        public virtual string? AfiliadoTelefonoContacto { get; set; }
        public virtual string? AfiliadoUbicacion { get; set; }
        public virtual string? AfiliadoCodigoPostal { get; set; }
        public virtual string? Bitacora { get; set; }
        public virtual string? MonitorId { get; set; }
        public virtual bool? ConfirmaAsignacion { get; set; } = false;
        public virtual bool? Cerrado { get; set; } = false;
        public virtual bool? AlarmaSonora { get; set; } = false;
        public virtual string? AfiliadoSexo { get; set; } = "O";
        public virtual int? AfiliadoEdad { get; set; } = 0;
        public virtual string? AfiliadoNosocomio { get; set; }
        public virtual string? Estado { get; set; }
        public virtual ModificacionDto Creado { get; set; } = new ModificacionDto();
        public virtual ModificacionDto Modificacion { get; set; } = new ModificacionDto();

       
        public long TimeOut { get; set; } = 0;
    }
}
