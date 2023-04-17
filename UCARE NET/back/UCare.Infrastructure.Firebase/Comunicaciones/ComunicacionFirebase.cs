using Google.Cloud.Firestore;

namespace UCare.Infrastructure.Firebase.Comunicaciones
{
    [FirestoreData]
    public class ComunicacionFirebase : EntityBaseFirebase<string?>
    {
        [FirestoreProperty]
        public string Titulo { get; set; }
        [FirestoreProperty]
        public string Mensaje { get; set; }
        [FirestoreProperty]
        public DateTime? FechaEnvio { get; set; }
        [FirestoreProperty]
        public bool Enviado { get; set; }
        [FirestoreProperty]
        public string Destinos { get; set; } = "";
        [FirestoreProperty]
        public string? DetalleEnvio { get; set; }
    }

    [FirestoreData]
    public class ComunicacionEnvioFirebase : AggregateRootFirebase<string?>
    {
        [FirestoreProperty]
        public string AfiliadoId { get; set; } = string.Empty;
        [FirestoreProperty]
        public string ComunicadoId { get; set; } = string.Empty;
        [FirestoreProperty]
        public string Estado { get; set; } = string.Empty;
    }
}
