using PhoneNumbers;
using UCare.Shared.Infrastructure.PhoneNumber;

namespace UCare.Infrastructure.SMS.Twilio
{
    public class ValidatePhoneNumber : IValidatePhoneNumber
    {
        public bool Validate(string phoneNumber, string countryCode)
        {
            var _phoneUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var phone = _phoneUtil.Parse(phoneNumber, countryCode);
                return _phoneUtil.IsValidNumber(phone);
            }
            catch (NumberParseException npex)
            {
                throw;
            }
            catch (Exception e)
            {
                throw;
            }
            return false;
        }
    }
}
