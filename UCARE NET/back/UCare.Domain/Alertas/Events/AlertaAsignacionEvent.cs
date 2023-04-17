using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaAsignacionEvent : DomainEvent
    {
        public string AfiliadoId { get; }
        public string MonitorId { get; }
        public AlertaAsignacionEvent(string aggregateId, string AfiliadoId, string MonitorId) : base(aggregateId, null, null)
        {
            this.AfiliadoId = AfiliadoId;
            this.MonitorId = MonitorId;
        }

        public AlertaAsignacionEvent(string aggregateId, string AfiliadoId, string MonitorId, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.AfiliadoId = AfiliadoId;
            this.MonitorId = MonitorId;
        }

        public override string EventName()
        {
            return "AlertaAsignacionEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaAsignacionEvent(aggregateId, body["AfiliadoId"], body["MonitorId"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "AfiliadoId", AfiliadoId }, { "MonitorId", MonitorId } };
        }
    }

    public class ConfirmarAsignacionEvent : DomainEvent
    {
        public string AfiliadoId { get; }
        public string MonitorId { get; }
        public ConfirmarAsignacionEvent(string aggregateId, string AfiliadoId, string MonitorId) : base(aggregateId, null, null)
        {
            this.AfiliadoId = AfiliadoId;
            this.MonitorId = MonitorId;
        }

        public ConfirmarAsignacionEvent(string aggregateId, string AfiliadoId, string MonitorId, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.AfiliadoId = AfiliadoId;
            this.MonitorId = MonitorId;
        }

        public override string EventName()
        {
            return "ConfirmarAsignacionEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new ConfirmarAsignacionEvent(aggregateId, body["AfiliadoId"], body["MonitorId"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "AfiliadoId", AfiliadoId }, { "MonitorId", MonitorId } };
        }
    }

}