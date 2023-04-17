using System.ComponentModel.DataAnnotations;
using UCare.Shared.Domain;

namespace UCare.Domain.Alertas
{
    public class AlertaHistorial : GQ.Data.Abstractions.Entity.EntityBase<string?>
    {
        [Required]
        public virtual string AlertId { get; set; } = string.Empty;

        [Required]
        public virtual string AfiliadoId { get; set; } = string.Empty;

        [Required]
        public virtual string? MonitorId { get; set; } = string.Empty;

        public bool? AlarmaSonora { get; set; } = false;
        public virtual string? Descripcion { get; set; } = string.Empty;

        [Required]
        public virtual string Estado { get; set; } = string.Empty;

        [Required]
        public virtual Modificacion Creado { get; set; } = new Modificacion();

        [Required]
        public GeoPosition Position { get; internal set; } = new GeoPosition();
    }


}
