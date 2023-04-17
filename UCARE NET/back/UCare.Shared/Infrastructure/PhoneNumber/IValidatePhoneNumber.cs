namespace UCare.Shared.Infrastructure.PhoneNumber
{
    public interface IValidatePhoneNumber
    {
        public bool Validate(string phoneNumber, string countryCode);
    }
}
