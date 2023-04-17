using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Users.Events
{
    public class NuevaComunicacionEvent : DomainEvent
    {
        public string ComunicacionId { get; }
        public string Titulo { get; }
        public string Mensaje { get; }

        public NuevaComunicacionEvent(string aggregateId, string ComunicacionId, string Titulo, string Mensaje) : base(aggregateId, null, null)
        {
            this.ComunicacionId = ComunicacionId;
            this.Titulo = Titulo;
            this.Mensaje = Mensaje;
        }

        public NuevaComunicacionEvent(string aggregateId, string ComunicacionId, string Titulo, string Mensaje, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.ComunicacionId = ComunicacionId;
            this.Titulo = Titulo;
            this.Mensaje = Mensaje;
        }

        public override string EventName()
        {
            return "NuevaComunicacionEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new NuevaComunicacionEvent(aggregateId, body["ComunicacionId"], body["Titulo"], body["Mensaje"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "ComunicacionId", ComunicacionId }, { "Titulo", Titulo }, { "Mensaje", Mensaje } };
        }
    }
}