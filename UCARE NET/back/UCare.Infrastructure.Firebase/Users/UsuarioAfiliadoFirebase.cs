using Google.Cloud.Firestore;

namespace UCare.Infrastructure.Firebase.Users
{
    [FirestoreData]
    public class UsuarioAfiliadoFirebase : EntityBaseFirebase<string?>
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
        public virtual string? Lenguaje { get; set; }
        [FirestoreProperty]
        public virtual string? Signature { get; set; }
        [FirestoreProperty]
        public virtual string NumeroIdentidad { get; set; }
        [FirestoreProperty]
        public virtual DateTime FechaNacimiento { get; set; }
        [FirestoreProperty]
        public virtual string Sexo { get; set; }
        [FirestoreProperty]
        public virtual string Token { get; set; }
        [FirestoreProperty]
        public virtual DireccionFirebase Direccion { get; set; } = new DireccionFirebase();
        [FirestoreProperty]
        public virtual GeoPositionFirebase? Position { get; set; } = new GeoPositionFirebase();
        [FirestoreProperty]
        public virtual string Celular { get; set; }
        [FirestoreProperty]
        public virtual string CodigoPais { get; set; }
        [FirestoreProperty]
        public virtual string Nosocomio { get; set; }
        [FirestoreProperty]
        public virtual List<string> Patologias { get; set; } = new List<string>();
        [FirestoreProperty]
        public virtual List<string> Medicacion { get; set; } = new List<string>();
        [FirestoreProperty]
        public virtual List<string> Alergias { get; set; } = new List<string>();
        [FirestoreProperty]
        public virtual List<ContactoFirebase> Contactos { get; set; } = new List<ContactoFirebase> { };
        [FirestoreProperty]
        public virtual AfiliacionFirebase Afiliacion { get; set; } = new AfiliacionFirebase();
        [FirestoreProperty]
        public virtual VerificacionFirebase VerificaEmail { get; set; } = new VerificacionFirebase();
        [FirestoreProperty]
        public virtual VerificacionFirebase VerificaTelefono { get; set; } = new VerificacionFirebase();
    }

    [FirestoreData]
    public class VerificacionFirebase
    {
        [FirestoreProperty]
        public bool Verificado { get; set; } = false;
        [FirestoreProperty]
        public string CodigoVerificacion { get; set; } = string.Empty;
    }

    [FirestoreData]
    public class GeoPositionFirebase
    {
        [FirestoreProperty]
        public virtual double? Lat { get; set; } = double.MaxValue;
        [FirestoreProperty]
        public virtual double? Lon { get; set; } = double.MaxValue;
    }

    [FirestoreData]
    public class DireccionFirebase
    {
        [FirestoreProperty]
        public virtual string? Calle { get; set; }
        [FirestoreProperty]
        public virtual string? Nro { get; set; }
        [FirestoreProperty]
        public virtual string? Piso { get; set; }
        [FirestoreProperty]
        public virtual string? Barrio { get; set; }
        [FirestoreProperty]
        public virtual string? Ciudad { get; set; }
        [FirestoreProperty]
        public virtual string? Departamento { get; set; }
        [FirestoreProperty]
        public virtual string? CodigoPostal { get; set; }
    }


    [FirestoreData]
    public class ContactoFirebase
    {
        [FirestoreProperty]
        public virtual string? Nombre { get; set; }
        [FirestoreProperty]
        public virtual string? Telefono { get; set; }
    }

    [FirestoreData]
    public class AfiliacionFirebase
    {
        [FirestoreProperty]
        public virtual string Empresa { get; set; }
        [FirestoreProperty]
        public virtual string Servicio { get; set; }
    }
}
