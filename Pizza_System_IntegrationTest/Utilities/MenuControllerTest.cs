using FluentAssertions;
using Pizza_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_System_IntegrationTest.Utilities
{
    public class MenuControllerTest : CustomApiFactory, IClassFixture<CustomApiFactory> 

    {
        public CustomApiFactory factory;
        
        public MenuControllerTest(CustomApiFactory factory)
        {
            this.factory = factory;
        }

        
        [Fact]
        public async void GetAll_Test()

        {
            // Arrange
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

            Menu menu2 = new Menu
            {
                Id = 2,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 8,
                Vegan = false,
                Price = 8
            };


            //Act
            var response = await factory.CreateClient().GetAsync("/v1/api/menus");
           
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void GetByid_Test()
        {
            // Arrange
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
           

            var result =  factory.AppDbContext.Menus.Add(menu1);
            factory.AppDbContext.SaveChanges();
           
            var client = factory.CreateClient();
            var response = await client.GetAsync("/v1/api/menus/1");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async void Post_Test()
        {
            // Arrange
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

            Menu menu6 = new Menu
            {
                Id = 11,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };
            
            // Generate the Jwt token with Role manager
            var token = new TestJwtToken().WithRole("Manager").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Act
            var response = await Client.PostAsync("/v1/api/menus", JsonContent.Create(menu1));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            //Act
            //var response = await factory.CreateClient().GetAsync("/v1/api/menus");


            //Act
            //var client = factory.CreateClient();//.PostAsync("/v1/api/menus/", JsonContent.Create(menu1));

            //var response = await client.PostAsync("/v1/api/menus/", JsonContent.Create(menu1));
            // Assert


        }

        [Fact]
        public async void Delete_Test()
        {
            // Arrange
            Menu menu4 = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            // generate the Jwt token with Manger role
            var token = new TestJwtToken().WithRole("Manager").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Act
            var response = await Client.DeleteAsync("/v1/api/menus/1");

           
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            /*var client = factory.CreateClient();
           var response = await client.DeleteAsync("/v1/api/menus/1");*/
        }

        [Fact]
        public async void Update_Test()
        {
            // Arrange
            Menu menu2 = new Menu
            {
                Id = 11, // change this from any number and back to 1 to work again
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };


            factory.AppDbContext.Menus.Add(menu2);
            factory.AppDbContext.SaveChanges();

            menu2.Price = 20;

            // generate the Jwt token with Manger role
            var token = new TestJwtToken().WithRole("Manager").Build();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            // Act
            var response = await Client.PutAsync("/v1/api/menus/11", JsonContent.Create(menu2));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
    }
}

