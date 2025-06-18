using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OmPlatform.Core;
using OmPlatform.Interfaces;
using OmPlatform.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace OmPlatformTest
{
    public static class Utilities
    {
        private static bool _dbInitialized;
        private static string _userToken;
        private static string _adminToken;

        private static List<Users> _users;
        private static List<Products> _products;
        private static List<Orders> _orders;

        public static void InitializeDbForTests(this WebAppFactory<Program> factory)
        {
            if (_dbInitialized) return;

            using var scope = factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DbAppContext>();
            db.Database.EnsureCreated();

            SeedData();
            db.Users.AddRange(_users);
            db.Products.AddRange(_products);
            db.Orders.AddRange(_orders);
            db.SaveChanges();

            _dbInitialized = true;
        }

        public static void InitializeClientForTests(this WebAppFactory<Program> factory)
        {
            var authService = factory.Services.GetService<IAuthService>();
            var user = _users[0].ToUserDto();

            // Generate user token
            _userToken = authService.GenerateJwtToken(user);

            // Generate same user token as admin
            user.Role = Constants.Admin;
            _adminToken = authService.GenerateJwtToken(user);
        }

        public static void SetAuthToken(this HttpClient client, bool isAdmin)
        {
            string token = isAdmin ? _adminToken : _userToken;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static void RemoveAuthToken(this HttpClient client)
        {
            client.DefaultRequestHeaders.Authorization = null;
        }

        public static StringContent ToJson(this object body)
        {
            return new StringContent(JsonSerializer.Serialize(body),Encoding.UTF8,"application/json");
        }

        public static void SeedData()
        {
            // Skip if already seeded
            if (_users != null) return;

            _users = new()
            {
                AddUser("user1", "user1@mail", "12345"),
                AddUser("user2", "user2@mail", "1234")
            };

            _products = new()
            {
                AddProduct("New1", 10, 0, "1"),
                AddProduct("New2", 20, 0, "2"),
                AddProduct("Test1", 30, 3, "2"),
                AddProduct("Test2", 40, 2, "2"),
                AddProduct("Test3", 50, 3, "2")
            };

            _orders = new()
            {
                AddOrder(10, _users[0].Id),
                AddOrder(20, _users[1].Id)
            };
        }

        private static Users AddUser(string name, string email, string password)
        {
            var hasher = new PasswordHasher<object>();
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = hasher.HashPassword(null, password),
                Role = Constants.User
            };
        }

        private static Products AddProduct(string name, int price, int stock, string category)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Price = price,
                Stock = stock,
                Category = category
            };
        }

        private static Orders AddOrder(int price, Guid userId)
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                Status = "Pending",
                TotalPrice = price,
                UserId = userId,
                OrderItems = []
            };
        }

        public static List<Users> GetSeededUsers() => _users;
        public static List<Products> GetSeededProducts() => _products;
        public static List<Orders> GetSeededOrders() => _orders;
    }
}
