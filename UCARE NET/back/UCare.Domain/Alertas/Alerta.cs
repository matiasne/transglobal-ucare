using GQ.Data.Abstractions.Entity;
using GQ.Log;
using System.ComponentModel.DataAnnotations;
using UCare.Domain.Alertas.Events;
using UCare.Domain.Users;
using UCare.Shared.Domain.Auth;
using UCare.Shared.Domain.Locations;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Domain.Alertas
{
    public class Alerta : Shared.Domain.EntityBase<string?>
    {
        public Alerta()
        {
            Estado = Estados.SinAsignar;
        }
        public virtual GeoPosition Position { get; set; } = new GeoPosition();

        [Required]
        public virtual string AfiliadoId { get; set; } = string.Empty;
        public virtual string? AfiliadoNombreCompleto { get; set; }
        public virtual string? AfiliadoTelefonoContacto { get; set; }
        public virtual string? AfiliadoUbicacion { get; set; }
        public virtual string? AfiliadoCodigoPostal { get; set; }
        public virtual string? AfiliadoSexo { get; set; }
        public virtual int? AfiliadoEdad { get; set; }
        public virtual string? AfiliadoNosocomio { get; set; }

        public virtual string? Bitacora { get; set; }
        public virtual string? MonitorId { get; set; }
        public virtual bool? ConfirmaAsignacion { get; set; } = false;
        public virtual bool? Cerrado { get; set; } = false;
        public virtual bool? AlarmaSonora { get; set; } = false;

        private AlertaHistorial? Historial;

        public AlertaHistorial? GetHistorial() { return Historial; }

        public void CambiarPosicion(GeoPosition geoPosition, AuthUser user)
        {
            if (!geoPosition.Lat.HasValue || geoPosition.Lat < -90 || geoPosition.Lat > 90)
            {
                throw new ArgumentException("Fuera de ranfo valido", "Latitud");
            }

            if (!geoPosition.Lon.HasValue || geoPosition.Lon < -180 || geoPosition.Lon > 180)
            {
                throw new ArgumentException("Fuera de ranfo valido", "Longitud");
            }

            Record(new AlertaChangePositionEvent(Id, geoPosition.Lat.Value, geoPosition.Lon.Value));

            Position = geoPosition;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = user?.Id };

            CreateHistorial();
            Historial.Descripcion = $"Cambio de Posicion";
            Historial.Position = geoPosition;
        }

        public void CambairEstado(string EstadoNuevo, AuthUser user)
        {
            if ((new string[] { Estados.FalsaAlarma, Estados.SinAsignar, Estados.Urgencia, Estados.Emergencia }).Any(x => x == EstadoNuevo))
            {
                if ((new string[] { Estados.FalsaAlarma, Estados.Urgencia, Estados.Emergencia }).Any(x => x == EstadoNuevo) && MonitorId != user.Id)
                {
                    throw new Exception("No tiene permisos para cambiar el estado de esta alerta porque no esta asignado a ella");
                }

                Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = user.Id };

                Record(new AlertaChangeStateEvent(Id, AfiliadoId, EstadoNuevo, Estado));

                CreateHistorial();
                Historial.Descripcion = $"Cambio de Estado de {Estado} a {EstadoNuevo}";
                Historial.Estado = EstadoNuevo;

                Estado = EstadoNuevo;
            }
            else
            {
                throw new Exception("Estado Incorrecto");
            }
        }

        public void AsignarMonitor(string monitorId, AuthUser? user)
        {
            MonitorId = monitorId;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = user?.Id };

            CreateHistorial();
            Historial.Descripcion = String.IsNullOrWhiteSpace(monitorId) ? "Desasignacion de Monitor" : "Asignacion de Monitor";
            Historial.MonitorId = monitorId;

            Record(new AlertaAsignacionEvent(Id, AfiliadoId, monitorId));
        }

        public void ConfirmarAsignacion(string monitorId)
        {
            if (MonitorId != monitorId)
            {
                throw new Exception("No se puede confirmar porque la alerta no esta asignada a este usuario");
            }

            MonitorId = monitorId;
            ConfirmaAsignacion = true;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = monitorId };

            CreateHistorial();
            Historial.Descripcion = "Confirmar Asignacion de Monitor";
            Historial.MonitorId = monitorId;

            Record(new ConfirmarAsignacionEvent(Id, AfiliadoId, monitorId));
        }

        public void CambairEstadoAlarma(bool alarma, AuthUser authUser)
        {
            if (MonitorId != authUser.Id)
            {
                throw new Exception("No se puede activar o desactivar la alarma sonora. La alerta no esta asignada a este usuario");
            }
            AlarmaSonora = alarma;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = authUser.Id };

            CreateHistorial();
            Historial.Descripcion = alarma ? "Activar alarma Sonora" : "Desactivar alarma Sonora";
            Historial.AlarmaSonora = AlarmaSonora;

            Record(new AlertaAlarmaSonoraEvent(Id, AfiliadoId, AlarmaSonora ?? false));
        }

        public void FinalizarAsistencia(string bitacora, AuthUser authUser)
        {
            if (MonitorId != authUser.Id)
            {
                throw new Exception("No se puede finalizar la asistencia. La alerta no esta asignada a este usuario");
            }
            Bitacora = bitacora;

            if (AlarmaSonora ?? false)
                Record(new AlertaAlarmaSonoraEvent(Id, AfiliadoId, false));

            AlarmaSonora = false;
            Cerrado = true;
            Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow, UsuarioId = authUser.Id };

            CreateHistorial();

            Historial.Descripcion = "Finaliza la asistencia";
            Historial.AlarmaSonora = AlarmaSonora;

            Record(new AlertaFinalizarEvent(Id, AfiliadoId));
        }

        public void CreateEvent()
        {
            Record(new AlertaCreateEvent(Id, AfiliadoId, Position.Lat.ToString(), Position.Lon.ToString()));
        }

        public static Alerta Create(UsuarioAfiliado usuario, GeoPosition position, List<Location> locations)
        {
            Alerta alerta = new Alerta();
            alerta.AfiliadoId = usuario.Id!;
            alerta.AfiliadoNombreCompleto = usuario.UsuarioNombre;
            alerta.AfiliadoTelefonoContacto = usuario.Celular;
            alerta.AfiliadoUbicacion = usuario.Direccion?.ToString() ?? ""; //  locations.FirstOrDefault()?.FormattedAddress ?? "";
            alerta.AfiliadoCodigoPostal = usuario.Direccion?.CodigoPostal;
            alerta.AfiliadoSexo = usuario.Sexo;
            try
            {
                alerta.AfiliadoEdad = DateTime.UtcNow.AddTicks(usuario.FechaNacimiento.Ticks * -1).Year;
            }
            catch
            {
                alerta.AfiliadoEdad = 0;
            }
            alerta.AfiliadoNosocomio = usuario.Nosocomio;
            alerta.AlarmaSonora = false;
            alerta.Cerrado = false;
            alerta.MonitorId = null;
            alerta.Position = position;
            alerta.Estado = Estados.SinAsignar;
            alerta.Creado = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow };
            alerta.Modificacion = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow };
            alerta.TenantId = usuario.TenantId;
            alerta.CreateHistorial();

            return alerta;
        }

        private void CreateHistorial()
        {
            Historial = new AlertaHistorial
            {
                Descripcion = $"Creacion Alerta",
                AlertId = Id,
                AfiliadoId = AfiliadoId,
                MonitorId = MonitorId,
                AlarmaSonora = AlarmaSonora,
                Estado = Estado,
                Position = Position,
                Creado = new Shared.Domain.Modificacion { Modificado = DateTime.UtcNow }
            };
        }

    }

    public class GeoPosition : IEntity
    {
        public virtual double? Lat { get; set; } = double.MaxValue;
        public virtual double? Lon { get; set; } = double.MaxValue;
    }

}
