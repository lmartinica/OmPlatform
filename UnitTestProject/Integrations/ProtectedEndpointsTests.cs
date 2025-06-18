using static OmPlatform.Core.Constants;
using System.Net;

namespace OmPlatformTest.Integrations
{
    public class ProtectedEndpointsTests : IClassFixture<WebAppFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory<Program> _factory;

        public ProtectedEndpointsTests(WebAppFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
            _factory.InitializeDbForTests();
            _factory.InitializeClientForTests();
        }

        public static IEnumerable<object[]> ProtectedEndpoints()
        {
            return new List<object[]>
            {
               // Orders
               new object[] { RouteOrder, HttpMethod.Get },
               new object[] { RouteOrder, HttpMethod.Post },
               new object[] { RouteOrderId, HttpMethod.Get },
               new object[] { RouteOrderId, HttpMethod.Patch },
               new object[] { RouteOrderId, HttpMethod.Delete },

               // Users
               new object[] { RouteUser, HttpMethod.Get },
               new object[] { RouteUserMe, HttpMethod.Get },
               new object[] { RouteUserId, HttpMethod.Get },
               new object[] { RouteUserId, HttpMethod.Patch },
               new object[] { RouteUserId, HttpMethod.Delete },

               // Products
               new object[] { RouteProduct, HttpMethod.Get },
               new object[] { RouteProduct, HttpMethod.Post },
               new object[] { RouteProductId, HttpMethod.Get },
               new object[] { RouteProductId, HttpMethod.Patch },
               new object[] { RouteProductId, HttpMethod.Delete },

               // Reports
               new object[] { RouteReportSale, HttpMethod.Get },
               new object[] { RouteReportProduct, HttpMethod.Get },
               new object[] { RouteReportCustomer, HttpMethod.Get },
            };
        }

        public static IEnumerable<object[]> ProtectedAdminEndpoints()
        {
            return new List<object[]>
            {
               // Products
               new object[] { RouteProduct, HttpMethod.Post },
               new object[] { RouteProductId, HttpMethod.Patch },
               new object[] { RouteProductId, HttpMethod.Delete },

               // Reports
               new object[] { RouteReportSale, HttpMethod.Get },
               new object[] { RouteReportProduct, HttpMethod.Get },
               new object[] { RouteReportCustomer, HttpMethod.Get },

               // Users
               new object[] { RouteUser, HttpMethod.Get },
            };
        }

        [Theory]
        [MemberData(nameof(ProtectedEndpoints))]
        public async Task ProtectedEndpoints_NoToken_NoAccess(string url, HttpMethod method)
        {
            // Arrange
            _client.RemoveAuthToken();

            // Act
            var httpRequest = new HttpRequestMessage(method, url);
            var response = await _client.SendAsync(httpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Theory]
        [MemberData(nameof(ProtectedEndpoints))]
        public async Task ProtectedEndpoints_WithAdminToken_NoAccess(string url, HttpMethod method)
        {
            // Arrange
            _client.SetAuthToken(true);

            // Act
            var httpRequest = new HttpRequestMessage(method, url);
            var response = await _client.SendAsync(httpRequest);

            // Assert
            Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        
        [Theory]
        [MemberData(nameof(ProtectedAdminEndpoints))]
        public async Task ProtectedAdminEndpoints_WithUserToken_NoAccess(string url, HttpMethod method)
        {
            // Arrange
            _client.SetAuthToken(false);

            // Act
            var httpRequest = new HttpRequestMessage(method, url);
            var response = await _client.SendAsync(httpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
