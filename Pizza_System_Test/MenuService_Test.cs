using Pizza_System;
using Pizza_System.Services;
using Moq;
using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;
using Pizza_System.Model;
using Pizza_System.Controllers;
using Sieve.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Pizza_System.Db;

namespace Pizza_System_Test
{
    public class MenuService_Test
    {
        public readonly Mock<AppDbContext> mockDbContext;
        public readonly MenuService menuService;


        public MenuService_Test()
        {

            mockDbContext = new Mock<AppDbContext>();
            menuService = new MenuService(mockDbContext.Object);
        }

        [Fact]
        public void GetMenuById_Test()
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


            // ARRANGE
            //var mockSet = new Mock<DbSet<Menu>>();

            var mockContext = new Mock<AppDbContext>();
            //menuService()
           // mockContext.Setup(m => m.Menus).Returns(mockSet.Object);

            var menuService = new MenuService(mockContext.Object);
            menuService.AddMenu(menu);


            //mockSet.Verify(m => m.Add(It.IsAny<Menu>()), Times.Once());
            //mockContext.Verify(m => m.SaveChanges(), Times.Once());

        }
    }
}