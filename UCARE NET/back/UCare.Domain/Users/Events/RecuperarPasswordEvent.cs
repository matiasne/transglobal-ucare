using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Users.Events
{
    public class RecuperarPasswordEvent : DomainEvent
    {
        public string Rol { get; }
        public string Nombre { get; }
        public string Code { get; }
        public string Signature { get; }
        public DateTime Expiration { get; }

        public RecuperarPasswordEvent(string aggregateId, string rol, string nombre, string code, DateTime expiration, string signature) : base(aggregateId, null, null)
        {
            Rol = rol;
            Nombre = nombre;
            Code = code;
            Expiration = expiration;
            Signature = signature;
        }

        public RecuperarPasswordEvent(string aggregateId, string rol, string nombre, string code, DateTime expiration, string signature, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            Rol = rol;
            Nombre = nombre;
            Code = code;
            Expiration = expiration;
            Signature = signature;
        }

        public override string EventName()
        {
            return "RecuperarPasswordEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new RecuperarPasswordEvent(aggregateId, body["Rol"], body["Nombre"], body["Code"], new DateTime(long.Parse(body["Expiration"])), body["Signature"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "Rol", Rol }, { "Nombre", Nombre }, { "Code", Code }, { "Expiration", Expiration.Ticks.ToString() }, { "Signature", Signature } };
        }
    }
}