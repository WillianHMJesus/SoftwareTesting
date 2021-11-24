using Api.Tests.Config;
using Api.ViewModels;
using Features.Tests.OrderBy;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Api.Tests
{
    [TestCaseOrderer("Features.Tests.OrderBy.PriorityOrderer", "Features.Tests")]
    [Collection(nameof(IntegrationTestsFixtureCollection))]
    public class OrderApiTests
    {
        private readonly IntegrationTestsFixture<StartupTesting> _testsFixture;

        public OrderApiTests(IntegrationTestsFixture<StartupTesting> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Add Item New Order"), TestPriority(1)]
        [Trait("Category", "Api Integration - Order")]
        public async Task AddItem_NewOrder_MustResturnWithSuccess()
        {
            //Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("5e4a4bbd-99d0-4b07-87d3-49324599c2fb"),
                Quantity = 2
            };

            await _testsFixture.LoginApi();
            _testsFixture.Client.AssignAccessToken(_testsFixture.AccessToken);

            //Act
            var postResponse = await _testsFixture.Client.PostAsJsonAsync("api/cart", itemInfo);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remove Item Existing Order"), TestPriority(2)]
        [Trait("Category", "Api Integration - Order")]
        public async Task RemoveItem_ExistingOrder_MustResturnWithSuccess()
        {
            //Arrange
            var productId = new Guid("21583148-21d7-4499-bcd8-162c079232fe");
            await _testsFixture.LoginApi();
            _testsFixture.Client.AssignAccessToken(_testsFixture.AccessToken);

            //Act
            var deleteResponse = await _testsFixture.Client.DeleteAsync($"api/cart/{productId}");

            //Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
