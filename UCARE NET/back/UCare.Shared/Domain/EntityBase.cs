using GQ.Data.Abstractions.Entity;

namespace UCare.Shared.Domain
{
    public interface IEntityBase
    {
        Modificacion Creado { get; set; }
        Modificacion Modificacion { get; set; }
    }

    public class EntityBase<T> : TenantObject<T>, IEntityBase
    {
        public virtual string Estado { get; set; } = ValueObjects.Estados.Desactivo;
        public virtual Modificacion Creado { get; set; } = new Modificacion();
        public virtual Modificacion Modificacion { get; set; } = new Modificacion();
    }

    public class Modificacion : IEntity
    {
        public string? UsuarioId { get; set; }
        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
