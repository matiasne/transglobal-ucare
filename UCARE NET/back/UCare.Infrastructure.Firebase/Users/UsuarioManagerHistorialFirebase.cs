using Google.Cloud.Firestore;

namespace UCare.Infrastructure.Firebase.Users
{
    [FirestoreData]
    public class UsuarioManagerHistorialFirebase : AggregateRootFirebase<string>
    {
        [FirestoreProperty]
        public virtual DateTime Fecha { get; set; } = DateTime.UtcNow;
        [FirestoreProperty]
        public virtual string Accion { get; set; } = string.Empty;
        [FirestoreProperty]
        public virtual string AlertId { get; set; } = string.Empty;
        [FirestoreProperty]
        public virtual string AfiliadoId { get; set; } = string.Empty;
    }
}
