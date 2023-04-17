using GQ.Architecture.DDD.Domain.Bus.Event;

namespace UCare.Domain.Alertas.Events
{
    public class AlertaChangePositionEvent : DomainEvent
    {
        public double Lat { get; }
        public double Lon { get; }

        public AlertaChangePositionEvent(string aggregateId, double Lat, double Lon) : base(aggregateId, null, null)
        {
            this.Lat = Lat;
            this.Lon = Lon;
        }

        public AlertaChangePositionEvent(string aggregateId, double Lat, double Lon, string eventId, string occurredOn) : base(aggregateId, eventId, occurredOn)
        {
            this.Lat = Lat;
            this.Lon = Lon;
        }

        public override string EventName()
        {
            return "AlertaChangePositionEvent";
        }

        public override DomainEvent FromPrimitives(string aggregateId, Dictionary<string, string> body, string eventId, string occurredOn)
        {
            return new AlertaChangePositionEvent(aggregateId, double.Parse(body["Lat"]), double.Parse(body["Lat"]), eventId, occurredOn);
        }

        public override Dictionary<string, string> ToPrimitives()
        {
            return new Dictionary<string, string> { { "Lat", Lat.ToString() }, { "Lon", Lon.ToString() } };
        }
    }
}