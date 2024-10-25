using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Pizza_System.Model;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Pizza_System_IntegrationTest.Utilities
{
    /* Test methods fail in this test because in my OrderController 
        I am retrieving current logged in user from HtppContext
    */
    public class OrderController_Test : CustomApiFactory, IClassFixture<CustomApiFactory>

    {
        private readonly CustomApiFactory factory;
        public OrderController_Test(CustomApiFactory factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async void GetAll_Orders_Test()
        {
            //var s = new TestJwtToken().WithUserName
            Order order = new Order
            {
                OrderId= 1,
                MenuId= 1,
                DeliveryOption="Collection",
                PhoneNumber = "07485217458",
                Address1 = "123 Hight road",
                Address2 = "Islington",
                Street = "Hight road",
                PostCode = "N12 2SF",
                UserId = "f36dfe7a-778a-465b-9cdf-7bdb7b28994e"

            };

            var token = new TestJwtToken().WithRole("Customer").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync("/v1/api/orders");
            //var rresponse =await Client.GetAsync("/v1/api/orders");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
        }

        [Fact]
        public async void GetOrderById_WithOut_AuthorisationTest()
        {

            Order order = new Order
            {
                OrderId = 1,
                MenuId = 1,
                DeliveryOption = "Delivery",
                PhoneNumber = "07485217458",
                Address1 = "123 Hight road",
                Address2 = "Islington",
                Street = "Hight road",
                PostCode = "N12 2SF",
                UserId = "f36dfe7a-778a-465b-9cdf-7bdb7b28994e"

            };

            factory.AppDbContext.Orders.Add(order);
            var client = factory.CreateClient();
            var response = await client.GetAsync("/v1/api/orders/1/1");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void GetOrderById_Test()
        {

            Order order1 = new Order
            {
                OrderId = 1,
                MenuId = 1,
                DeliveryOption = "Delivery",
                PhoneNumber = "07485217458",
                Address1 = "123 Hight road",
                Address2 = "Islington",
                Street = "Hight road",
                PostCode = "N12 2SF",
                UserId = "f36dfe7a-778a-465b-9cdf-7bdb7b28994e"

            };

            var token = new TestJwtToken().WithRole("Customer").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.GetAsync("/v1/api/orders/1/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            /*factory.AppDbContext.Orders.Add(order);
            var client = factory.CreateClient();
            var response = await client.GetAsync("/v1/api/orders/1/1");
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);*/
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void Post_Test()
        {

            Order order2 = new Order
            {
                OrderId = 2,
                MenuId = 1,
                DeliveryOption = "Delivery",
                Address1 = "123 Hight road",
                Address2 = "Islington",
                Street = "Hight road",
                PostCode = "N12 2SF",
                UserId = "f36dfe7a-778a-465b-9cdf-7bdb7b28994e"

            };

            Menu menu1 = new Menu
            {
                Id = 15,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            factory.AppDbContext.Menus.Add(menu1);

            var token = new TestJwtToken().WithRole("Customer").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.PostAsync("/v1/api/orders", JsonContent.Create(order2));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async void Delete_Test()
        {

            Order order3 = new Order
            {
                OrderId = 3,
                MenuId = 1,
                DeliveryOption = "Delivery",
                PhoneNumber = "07485217458",
                Address1 = "123 Hight road",
                Address2 = "Islington",
                Street = "Hight road",
                PostCode = "N12 2SF",
                UserId = "f36dfe7a-778a-465b-9cdf-7bdb7b28994e"

            };

            Menu menu1 = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            factory.AppDbContext.Menus.Add(menu1);

            var token = new TestJwtToken().WithRole("Customer").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await Client.DeleteAsync("/v1/api/orders/3/1");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async void Should_Reject_Unauthenticated_Users()
        {
            // Arrange
            var token = new TestJwtToken().WithRole("Customer").Build();
            //Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Act
            var response = await Client.GetAsync("/v1/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async void Should_Reject_Unauthorised_Users()
        {
            // Arrange
            var token = new TestJwtToken().WithUserName("Customer@hotmail.com")
                .WithRole("Customer")
                .Build(); 
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await Client.GetAsync("/v1/api/orders");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}
