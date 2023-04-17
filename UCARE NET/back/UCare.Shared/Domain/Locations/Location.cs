namespace UCare.Shared.Domain.Locations
{
    public class Location
    {
        public virtual string? StreetNumber { get; set; }
        public virtual string? StreetName { get; set; }
        public virtual string? Neighborhood { get; set; }
        public virtual string? Locality { get; set; }
        public virtual string? Province { get; set; }
        public virtual string? Country { get; set; }
        public virtual string? FormattedAddress { get; set; }
        public virtual string? PostalCode { get; set; }
        public virtual LocationTypes LocationType { get; set; }
    }
    public enum LocationTypes
    {
        Level1,
        Level2,
        Level3,
        Level4,
    }
}
