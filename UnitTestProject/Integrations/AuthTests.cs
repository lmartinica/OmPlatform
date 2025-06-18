using System.Net;

namespace OmPlatformTest.Integrations
{
    public class AuthTests: IClassFixture<WebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory<Program> _factory;

        public AuthTests(WebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
            _factory.InitializeDbForTests();
        }

        [Fact]
        public async Task Post_Login_Success()
        {
            // Arrange
            var body = new { email = "user1@mail", password = "12345" };

            // Act
            var response = await _client.PostAsync("/auth/login", body.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Post_Login_InvalidCredentials()
        {
            // Arrange
            var body = new { email = "user1@mail", password = "wrongpass" };

            // Act
            var response = await _client.PostAsync("/auth/login", body.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_UserRegister_Success()
        {
            // Arrange
            var body = new { name = "user3", email = "user3@mail", password = "12345" };

            // Act
            var response = await _client.PostAsync("/auth/register", body.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Post_UserRegister_InvalidExistingEmail()
        {
            // Arrange
            var body = new { name = "user1", email = "user1@mail", password = "12345" };

            // Act
            var response = await _client.PostAsync("/auth/register", body.ToJson());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
