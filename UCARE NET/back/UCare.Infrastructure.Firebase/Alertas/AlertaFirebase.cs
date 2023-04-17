using Google.Cloud.Firestore;
using UCare.Infrastructure.Firebase.Users;

namespace UCare.Infrastructure.Firebase.Alertas
{
    [FirestoreData]
    public class AlertaFirebase : EntityBaseFirebase<string?>
    {
        [FirestoreProperty]
        public virtual GeoPositionFirebase Position { get; set; } = new GeoPositionFirebase();
        [FirestoreProperty]
        public virtual string AfiliadoId { get; set; } = string.Empty;
        [FirestoreProperty]
        public virtual string? AfiliadoNombreCompleto { get; set; }
        [FirestoreProperty]
        public virtual string? AfiliadoTelefonoContacto { get; set; }
        [FirestoreProperty]
        public virtual string? AfiliadoUbicacion { get; set; }
        [FirestoreProperty]
        public virtual string? AfiliadoCodigoPostal { get; set; }
        [FirestoreProperty]
        public virtual string? Bitacora { get; set; }
        [FirestoreProperty]
        public virtual string? MonitorId { get; set; }
        [FirestoreProperty]
        public virtual bool? ConfirmaAsignacion { get; set; } = false;
        [FirestoreProperty]
        public virtual bool? Cerrado { get; set; } = false;
        [FirestoreProperty]
        public virtual bool? AlarmaSonora { get; set; } = false;
        [FirestoreProperty]
        public virtual string? AfiliadoSexo { get; set; }
        [FirestoreProperty]
        public virtual int? AfiliadoEdad { get; set; }
        [FirestoreProperty]
        public virtual string? AfiliadoNosocomio { get; set; }
    }

    [FirestoreData]
    public class AlertaHistorialFirebase : AggregateRootFirebase<string>
    {
        [FirestoreProperty]
        public virtual string? AlertId { get; set; } = string.Empty;

        [FirestoreProperty]
        public virtual string? AfiliadoId { get; set; } = string.Empty;

        [FirestoreProperty]
        public virtual string? MonitorId { get; set; } = string.Empty;
        [FirestoreProperty]
        public virtual string? Descripcion { get; set; } = string.Empty;
        [FirestoreProperty]
        public virtual string Estado { get; set; } = string.Empty;
        [FirestoreProperty]
        public virtual ModificacionFirebase Creado { get; set; } = new ModificacionFirebase();
        [FirestoreProperty]
        public virtual bool? AlarmaSonora { get; set; } = false;
        [FirestoreProperty]
        public GeoPositionFirebase Position { get; internal set; } = new GeoPositionFirebase();
    }
}