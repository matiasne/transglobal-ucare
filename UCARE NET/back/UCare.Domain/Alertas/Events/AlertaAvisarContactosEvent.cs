using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaAvisarContactosEvent : DomainEvent
    {
        public string UsuarioId { get; }

        public AlertaAvisarContactosEvent(string aggregateId, string UsuarioId) : base(aggregateId, null, null)
        {
            this.UsuarioId = UsuarioId;
        }

        public AlertaAvisarContactosEvent(string aggregateId, string UsuarioId, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.UsuarioId = UsuarioId;
        }

        public override string EventName()
        {
            return "AlertaAvisarContactosEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaAvisarContactosEvent(aggregateId, body["UsuarioId"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "UsuarioId", UsuarioId } };
        }
    }
}