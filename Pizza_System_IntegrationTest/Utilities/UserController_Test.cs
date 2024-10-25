using Docker.DotNet.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_System.Dtos;
using Pizza_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_System_IntegrationTest.Utilities
{
    public class UserController_Test : IClassFixture<CustomApiFactory>

    {
        public CustomApiFactory factory;
        private readonly UserManager<User> userManager; //= new;
        public UserController_Test(CustomApiFactory factory)
        {
            this.factory = factory;
            
            
        }

        [Fact]
        public async void Register_Test()
        {
            var user = new RegisterRequest
            {
                FirstName = "Ali",
                LastName = "Hud",
                Email = "Ali@hotmail.com",
                Password = "Ali@hotmail.com"
            };

            var client = factory.CreateClient();
            var response = await client.PostAsync("/v1/api/users/register", JsonContent.Create(user));

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
        }

        // testing with existing email should return conflic
        [Fact]
        public async void Register_With_Existing_Email_Test()
        {
            var user = new RegisterRequest
            {
                FirstName = "Ali",
                LastName = "Hud",
                Email = "Ali@hotmail.com",
                Password = "Ali@hotmail.com"
            };

            var client = factory.CreateClient();
            var response = await client.PostAsync("/v1/api/users/register", JsonContent.Create(user));

            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            //response.Should().BeOfType<Created>();
        }

        [Fact]
        public async void Register_With_Invalid_Data_Test()
        {
            var user = new RegisterRequest
            {
                FirstName = "",
                LastName = "",
                Email = "Ali@hotmail.com",
                Password = "Ali@hotmail.com"
            };

            var client = factory.CreateClient();
            var response = await client.PostAsync("/v1/api/users/register", JsonContent.Create(user));

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            //response.Should().BeOfType<Created>();
        }

        [Fact]
        public async void Login_Test()
        {
            

            User user = new()
            {
                FirstName = "Mark",
                LastName = "John",
                Email = "Mark@hotmail.com",
                UserName = "Mark@hotmail.com",
                SecurityStamp = Guid.NewGuid().ToString()

            };


            var aUser = new LoginRequest
            {
                Email = "B@hotmail.com",
                Password = "B@hotmail.com"
            };

            //factory.AppDbContext.Add(aUser);
            //factory.AppDbContext.SaveChanges();

            var client = factory.CreateClient();
            var response = await client.PostAsync("/v1/api/users/login", JsonContent.Create(aUser));
            //await client.PostAsync("/v1/api/users/login", JsonContent.Create(aUser));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            //response.l
            //response.Should().BeOfType<Created>();
        }

        [Fact]
        public async void Login_With_Invalid_Data_Test()
        {
            // Arrange
            var aUser = new LoginRequest
            {
                Email = "",
                Password = ""
            };

            // Act
            var client = factory.CreateClient();
            var response = await client.PostAsync("/v1/api/users/login", JsonContent.Create(aUser));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
