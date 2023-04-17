using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Users.Events
{
    public class ChangeTelefonoEvent : DomainEvent
    {
        public string Nombre { get; }
        public string Codigo { get; }
        public string Telefono { get; }
        public string Signarure { get; }

        public ChangeTelefonoEvent(string aggregateId, string Nombre, string Codigo, string Telefono, string signarure) : base(aggregateId, null, null)
        {
            this.Nombre = Nombre;
            this.Codigo = Codigo;
            this.Telefono = Telefono;
            Signarure = signarure;
        }

        public ChangeTelefonoEvent(string aggregateId, string Nombre, string Codigo, string Telefono, string signarure, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.Nombre = Nombre;
            this.Codigo = Codigo;
            this.Telefono = Telefono;
            Signarure = signarure;
        }

        public override string EventName()
        {
            return "ChangeTelefonoEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new ChangeTelefonoEvent(aggregateId, body["Nombre"], body["Codigo"], body["Telefono"], body["Signarure"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "Nombre", Nombre }, { "Codigo", Codigo }, { "Telefono", Telefono }, { "Signarure", Signarure } };
        }
    }
}