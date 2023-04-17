using GQ.Architecture.DDD.Domain.Aggregate;
using UCare.Shared.Domain;

namespace UCare.Domain.Comunicaciones
{
    public class Comunicacion : Shared.Domain.EntityBase<string?>
    {
        public string Titulo { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime? FechaEnvio { get; set; }
        public bool Enviado { get; set; } = false;
        public string Destinos { get; set; } = "";
        public string? DetalleEnvio { get; set; }

        public void ModificarComunicacion(Comunicacion model, Modificacion modificacion)
        {
            Titulo = model.Titulo;
            Mensaje = model.Mensaje;
            FechaEnvio = model.FechaEnvio;
            Estado = model.Estado;
            Destinos = model.Destinos;
            DetalleEnvio = model.DetalleEnvio;
            Modificacion = modificacion;
        }
        public static Comunicacion CreateComunicacion(Comunicacion model, Modificacion modificacion)
        {
            return new Comunicacion
            {
                Creado = modificacion,
                Modificacion = modificacion,
                FechaEnvio = model.FechaEnvio ?? DateTime.UtcNow.AddDays(-1),
                Titulo = model.Titulo,
                Mensaje = model.Mensaje,
                Destinos = model.Destinos,
                TenantId = model.TenantId,
                Estado = model.Estado,
                Enviado = false,
                DetalleEnvio = "",
            };
        }

    }

    public class ComunicacionEnvio : AggregateRoot<string?>
    {
        public string AfiliadoId { get; set; } = string.Empty;
        public string ComunicadoId { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
