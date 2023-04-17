using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaCreateEvent : DomainEvent
    {
        public string UsuarioId { get; }
        public double lat { get; }
        public double lon { get; }

        public AlertaCreateEvent(string aggregateId, string UsuarioId, string lat, string lon) : base(aggregateId, null, null)
        {
            this.UsuarioId = UsuarioId;
            this.lat = double.Parse(lat);
            this.lon = double.Parse(lon);
        }

        public AlertaCreateEvent(string aggregateId, string UsuarioId, string lat, string lon, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.UsuarioId = UsuarioId;
            this.lat = double.Parse(lat);
            this.lon = double.Parse(lon);
        }

        public override string EventName()
        {
            return "AlertaCreateEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaCreateEvent(aggregateId, body["UsuarioId"], body["lat"], body["lon"], eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "UsuarioId", UsuarioId }, { "lat", lat.ToString() }, { "lon", lon.ToString() } };
        }
    }
}