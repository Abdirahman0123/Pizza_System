using Pizza_System.Model;
using Microsoft.AspNetCore.JsonPatch;

namespace Pizza_System.Services
{
    public interface IMenuService
    {
        public IQueryable<Menu> GetMenus();
        public Task<Menu> GetMenuById(int id);


        public Task<Menu> AddMenu(Menu menu);

        public Task DeleteMenu(int id);

        public Task<Menu> UpdateMenu(int id, Menu menu);

        public Task<Menu> PartialUpdateMenu(int id, JsonPatchDocument<Menu> patch);
    }
}
