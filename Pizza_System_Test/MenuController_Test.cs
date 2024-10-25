using FluentAssertions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pizza_System.Controllers;
using Pizza_System.Model;
using Pizza_System.Services;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_System_Test
{
    public class MenuController_Test
    {
        public readonly Mock<IMenuService> mockMenuService;
        public readonly Mock<ISieveProcessor> sieveProcessor;
        public readonly MenuController menuController;


        public MenuController_Test()
        {
            mockMenuService = new Mock<IMenuService>();
            sieveProcessor = new Mock<ISieveProcessor>();
            menuController = new MenuController(mockMenuService.Object, sieveProcessor.Object);
        }
        [Fact]
        public void GetById_Test()
        {
            // Arrange
            var menu = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            mockMenuService.Setup(m => m.GetMenuById(menu.Id)).ReturnsAsync(menu);

            // Act
            var result = menuController.Get(menu.Id);

            var obj = (OkObjectResult)result.Result;

            // Assert 
            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public void Post_Method_Test()
        {
            // Arrange
            var menu = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            mockMenuService.Setup(m => m.AddMenu(menu)).ReturnsAsync(menu);

            // Act
            var result = menuController.Post(menu);

            var obj = (ObjectResult)result.Result;

            // Assert 
            Assert.Equal(201, obj.StatusCode);
        }

        [Fact] public void GetAll_Controller()
        {
            var menus = new List<Menu>().AsQueryable();
            mockMenuService.Setup(m => m.GetMenus()).Returns(menus);
            SieveModel sieveModel = new SieveModel();
            var result = menuController.GetAll(sieveModel);

            var obj = (ObjectResult)result.Result;

            Assert.Equal(200, obj.StatusCode);
        }

        [Fact]
        public void HttpDelete_Method_Test()
        {
            var menu = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            mockMenuService.Setup(d => d.DeleteMenu(menu.Id));

            var result = menuController.Delete(menu.Id);

            // return 404, dont know why
            var obj = (NoContentResult) result.Result;
            Assert.Equal(204, obj.StatusCode);
            /*
             mockContext.Verify(m => m.SaveChanges(), Times.Once());*/
        }

        [Fact] public void Put_Method_Test ()
        {
            var menu = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 12,
                Vegan = true,
                Price = 10
            };

            var updateMenu = new Menu
            {
                Id = 1,
                Name = "Funghi",
                Toppings = "Tomato Sauce · Mozzarella · Mushrooms · Thyme",
                Crust = "Thin",
                Size = 8,
                Vegan = true,
                Price = 8
            };

            mockMenuService.Setup(m => m.UpdateMenu(menu.Id, menu)).ReturnsAsync(updateMenu);

            var result = menuController.Put(menu.Id, menu);

            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
            //var obj = (ObjectResult)result.Result;

            //Assert.Equal(200, obj.StatusCode);
        }
        
    }

}

