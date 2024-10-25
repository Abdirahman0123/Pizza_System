using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Pizza_System.Db;
using Pizza_System.Model;

namespace Pizza_System.Services
{
    public class MenuService: IMenuService
    {
        private readonly AppDbContext context;

        public MenuService(AppDbContext context)
        {
            this.context = context;
        }

        public  IQueryable<Menu> GetMenus()
        {
            return context.Menus.ToList().AsQueryable();
            //return new List<Menu>();
        
        }

        public async Task<Menu> GetMenuById(int id)
        {
            return await context.Menus.FindAsync(id);
        }


        public async Task<Menu> AddMenu(Menu menu)
        {
             context.Menus.Add(menu);
            await context.SaveChangesAsync();
            return menu;
        }

        public async Task DeleteMenu(int id)
        {
            var menu = context.Menus.Find(id);
            context.Menus.Remove(menu);
            await context.SaveChangesAsync();
        }

        public async Task<Menu> UpdateMenu(int id, Menu newMenu)
        {
            // retrieve the menu using the id
            var oldMenu = await context.Menus.FindAsync(id);

            oldMenu.Name = newMenu.Name;
            oldMenu.Toppings = newMenu.Toppings;
            oldMenu.Crust   = newMenu.Crust;    
            oldMenu.Vegan = newMenu.Vegan;
            oldMenu.Price = newMenu.Price;
            await context.SaveChangesAsync();
            return newMenu;

        }

        public async Task<Menu> PartialUpdateMenu(int id, JsonPatchDocument<Menu> patch)
        {
            var menu = await context.Menus.FindAsync(id);
            patch.ApplyTo(menu);
            await context.SaveChangesAsync();
            return menu;
        }
    }
}
