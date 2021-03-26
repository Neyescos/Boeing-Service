using BLL.Services;
using NUnit.Framework;

namespace BLL_Tests
{
    [TestFixture]
    class PasswordEncodingServiceTests
    {
        private PasswordEncodingService encodingService;

        [SetUp]
        public void SetUp()
        {
            encodingService = new PasswordEncodingService();
        }

        [Test]
        public void PasswordEncodingService_Encode()
        {
            //Arrange
            var password = "MyCoolNewPassword123";
            //Act
            var encoded = encodingService.CalculateSHA256(password);
            //Assert
            Assert.NotNull(encoded);
            Assert.IsNotEmpty(encoded);
            Assert.AreEqual(encoded.Length, 32);
            
        }
    }
}
