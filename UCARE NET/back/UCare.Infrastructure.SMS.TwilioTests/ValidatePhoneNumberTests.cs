using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UCare.Infrastructure.SMS.Twilio.Tests
{
    [TestClass()]
    public class ValidatePhoneNumberTests
    {
        [TestMethod()]
        public void ValidateTest()
        {
            var v = new ValidatePhoneNumber();
            Assert.IsTrue(v.Validate("+543512065002", "+54"), "No se pudo validar telefono de argentina");
            Assert.IsTrue(v.Validate("+59899461691", "+598"), "No se pudo validar telefono de uruguay");
            Assert.IsTrue(v.Validate("+598093591700", "+598"), "No se pudo validar telefono de uruguay");
        }
    }
}