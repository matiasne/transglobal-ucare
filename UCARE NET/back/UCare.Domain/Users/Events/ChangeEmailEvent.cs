using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Users.Events
{
    public class ChangeEmailEvent : DomainEvent
    {
        public string Email { get; }
        public string Nombre { get; }
        public string Key { get; }

        public ChangeEmailEvent(string aggregateId, string Email, string Nombre, string key) : base(aggregateId, null, null)
        {
            this.Email = Email;
            this.Nombre = Nombre;
            this.Key = key;
        }

        public ChangeEmailEvent(string aggregateId, string Email, string Nombre, string key, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.Email = Email;
            this.Nombre = Nombre;
            this.Key = key;
        }

        public override string EventName()
        {
            return "ChangeEmailEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new ChangeEmailEvent(aggregateId, body["Email"], body["Nombre"], body["Key"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "Email", Email }, { "Nombre", Nombre }, { "Key", Key } };
        }
    }
}