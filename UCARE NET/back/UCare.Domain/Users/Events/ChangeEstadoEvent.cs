using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Users.Events
{
    public class ChangeEstadoEvent : DomainEvent
    {
        public string Email { get; }
        public string Nombre { get; }
        public string EstadoOld { get; }
        public string EstadoNew { get; }

        public ChangeEstadoEvent(string aggregateId, string Email, string Nombre, string EstadoOld, string EstadoNew) : base(aggregateId, null, null)
        {
            this.Email = Email;
            this.Nombre = Nombre;
            this.EstadoOld = EstadoOld;
            this.EstadoNew = EstadoNew;
        }

        public ChangeEstadoEvent(string aggregateId, string Email, string Nombre, string EstadoOld, string EstadoNew, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.Email = Email;
            this.Nombre = Nombre;
            this.EstadoOld = EstadoOld;
            this.EstadoNew = EstadoNew;
        }

        public override string EventName()
        {
            return "ChangeEstadoEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new ChangeEstadoEvent(aggregateId, body["Email"], body["Nombre"], body["EstadoOld"], body["EstadoNew"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "Email", Email }, { "Nombre", Nombre }, { "EstadoOld", EstadoOld }, { "EstadoNew", EstadoNew } };
        }
    }
}