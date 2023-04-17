using Google.Cloud.Firestore;

namespace UCare.Infrastructure.Firebase.Users
{
    [FirestoreData]
    internal class ComunicacionAfiliadoFirebase : AggregateRootFirebase<string?>
    {
        [FirestoreProperty]
        public string IdComunicado { get; set; }
        [FirestoreProperty]
        public string Titulo { get; set; }
        [FirestoreProperty]
        public string Mensaje { get; set; }
        [FirestoreProperty]
        public DateTime Fecha { get; set; }
        [FirestoreProperty]
        public string Estado { get; set; }
    }
}
