using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaChangeStateEvent : DomainEvent
    {
        public string AfiliadoId { get; }
        public string EstadoNuevo { get; }
        public string Estado { get; }

        public AlertaChangeStateEvent(string aggregateId, string AfiliadoId, string EstadoNuevo, string Estado) : base(aggregateId, null, null)
        {
            this.AfiliadoId = AfiliadoId;
            this.EstadoNuevo = EstadoNuevo;
            this.Estado = Estado;
        }

        public AlertaChangeStateEvent(string aggregateId, string AfiliadoId, string EstadoNuevo, string Estado, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.AfiliadoId = AfiliadoId;
            this.EstadoNuevo = EstadoNuevo;
            this.Estado = Estado;
        }

        public override string EventName()
        {
            return "AlertaChangeStateEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaChangeStateEvent(aggregateId, body["AfiliadoId"], body["EstadoNuevo"], body["Estado"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "AfiliadoId", AfiliadoId }, { "EstadoNuevo", EstadoNuevo }, { "Estado", Estado } };
        }
    }
}