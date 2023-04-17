using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaFinalizarEvent : DomainEvent
    {
        public string UsuarioId { get; }

        public AlertaFinalizarEvent(string aggregateId, string UsuarioId) : base(aggregateId, null, null)
        {
            this.UsuarioId = UsuarioId;
        }

        public AlertaFinalizarEvent(string aggregateId, string UsuarioId, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.UsuarioId = UsuarioId;
        }

        public override string EventName()
        {
            return "AlertaFinalizarEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaFinalizarEvent(aggregateId, body["UsuarioId"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "UsuarioId", UsuarioId } };
        }
    }
}