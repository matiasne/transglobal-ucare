using Google.Cloud.Firestore;
using UCare.Shared.Domain.ValueObjects;

namespace UCare.Infrastructure.Firebase
{
    public interface IEntityBaseFirebase
    {
        ModificacionFirebase Creado { get; set; }
        ModificacionFirebase Modificacion { get; set; }
    }

    [FirestoreData]
    public abstract class AggregateRootFirebase<T>
    {
        [FirestoreDocumentId]
        public virtual T Id { get; set; }
    }

    [FirestoreData]
    public abstract class TenantObjectFirebase<T> : AggregateRootFirebase<T>
    {
        [FirestoreProperty]
        public virtual string? TenantId { get; set; }
    }

    [FirestoreData]
    public abstract class EntityBaseFirebase<T> : TenantObjectFirebase<T>, IEntityBaseFirebase
    {
        [FirestoreProperty]
        public virtual string Estado { get; set; } = Estados.Desactivo;
        [FirestoreProperty]
        public virtual ModificacionFirebase Creado { get; set; } = new ModificacionFirebase();
        [FirestoreProperty]
        public virtual ModificacionFirebase Modificacion { get; set; } = new ModificacionFirebase();
    }

    [FirestoreData]
    public class ModificacionFirebase
    {
        [FirestoreProperty]
        public string? UsuarioId { get; set; }
        [FirestoreProperty]
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
