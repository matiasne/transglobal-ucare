using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaAlarmaSonoraEvent : DomainEvent
    {
        public string UsuarioId { get; }
        public bool Alarma { get; }

        public AlertaAlarmaSonoraEvent(string aggregateId, string UsuarioId, bool Alarma) : base(aggregateId, null, null)
        {
            this.UsuarioId = UsuarioId;
            this.Alarma = Alarma;
        }

        public AlertaAlarmaSonoraEvent(string aggregateId, string UsuarioId, bool Alarma, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.UsuarioId = UsuarioId;
            this.Alarma = Alarma;
        }

        public override string EventName()
        {
            return "AlertaAlarmaSonoraEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaAlarmaSonoraEvent(aggregateId, body["UsuarioId"], bool.Parse(body["Alarma"]), eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "UsuarioId", UsuarioId }, { "Alarma", Alarma.ToString() } };
        }
    }
}