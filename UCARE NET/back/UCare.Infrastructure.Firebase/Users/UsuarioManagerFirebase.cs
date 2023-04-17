using Google.Cloud.Firestore;
using GQ.Data.Abstractions.Entity;
using UCare.Infrastructure.Firebase;

namespace UCare.Infrastructure.Firebase.Users
{
    [FirestoreData]
    public class UsuarioManagerFirebase : EntityBaseFirebase<string?>
    {
        #region User Information

        [FirestoreProperty]
        public virtual string? Rol { get; set; }
        [FirestoreProperty]
        public virtual string? Email { get; set; }
        [FirestoreProperty]
        public virtual string? UsuarioNombre { get; set; }
        [FirestoreProperty]
        public virtual string? Password { get; set; }
        [FirestoreProperty]
        public virtual string? Salt { get; set; }
        #endregion

        [FirestoreProperty]
        public virtual List<string>? CodigoPostal { get; set; }
        [FirestoreProperty]
        public virtual string? UsuarioId { get; set; } // Usuario a quien pertenece
        [FirestoreProperty]
        public virtual MapaConfigFirebase Mapa { get; set; } = new MapaConfigFirebase();

    }
    [FirestoreData]
    public class MapaConfigFirebase
    {
        [FirestoreProperty]
        public virtual int Zoom { get; set; }
        [FirestoreProperty]
        public virtual GeoPositionFirebase Center { get; set; } = new GeoPositionFirebase();
    }
}

[FirestoreData]
public class UsuarioAuthFirebase : EntityBaseFirebase<string>
{
    [FirestoreProperty]
    public string IdKey { get; set; }

    [FirestoreProperty]
    public string UsuarioId { get; set; }
    [FirestoreProperty]
    public string Code { get; set; }
    [FirestoreProperty]
    public DateTime Expiration { get; set; }
}