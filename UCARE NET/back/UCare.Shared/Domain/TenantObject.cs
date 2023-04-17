using GQ.Architecture.DDD.Domain.Aggregate;

namespace UCare.Shared.Domain
{
    public interface ITenantObject
    {
        string? TenantId { get; set; }
    }

    public abstract class TenantObject<T> : AggregateRoot<T>, ITenantObject
    {
        public virtual string? TenantId { get; set; }
    }
}
