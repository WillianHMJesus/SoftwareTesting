using Api.ViewModels;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationTestsFixtureCollection))]
    public class IntegrationTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupTesting>>
    { }

    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly StoreAppFactory<TStartup> Factory;
        public HttpClient Client;
        public string AccessToken;

        public IntegrationTestsFixture()
        {
            Factory = new StoreAppFactory<TStartup>();
            Client = Factory.CreateClient();
        }

        public async Task LoginApi()
        {
            var userData = new LoginViewModel
            {
                Email = "test@test.com",
                Senha = "Test@123"
            };

            var response = await Client.PostAsJsonAsync("api/login", userData);
            response.EnsureSuccessStatusCode();
            AccessToken = await response.Content.ReadAsStringAsync();
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
