using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pizza_System.Model;
using Pizza_System.Services;
using Sieve.Models;
using Sieve.Services;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pizza_System.Controllers
{
    [Route("v1/api/menus")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        private readonly ISieveProcessor sieveProcessor;

        public MenuController(IMenuService menuService, ISieveProcessor sieveProcessor)
        {
            this.menuService = menuService;
            this.sieveProcessor = sieveProcessor;
        }

        
        [HttpGet]
        public  ActionResult<IQueryable<Menu>> GetAll([FromQuery] SieveModel sieveModel)
        {
            //var menus =  await menuService.GetMenus();
            var result =  sieveProcessor.Apply(sieveModel, menuService.GetMenus());
            return Ok(result);
        }

        // GET api/<MenuController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //return await menuService.GetMenuById(id);

            if (id <= 0)
            {
                return BadRequest($"Please enter correct id");
            }

            var menu = await menuService.GetMenuById(id);

            if (menu is null)
            {
                return NotFound("Record does not exists");
            }

            return Ok(menu);
        }

        
        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Menu menu)
        {
            var newMenu = await menuService.AddMenu(menu);

            return new ObjectResult(newMenu) { StatusCode = StatusCodes.Status201Created };
            

        }


        [Authorize(Roles = "Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Menu menu)
        {
            /*if (id <= 0)
            {
                return BadRequest("Please enter valid id");
            }*/
            //check if a record exists before attemping to 
            var menuExits = await menuService.GetMenuById(id);

            if (menuExits is null)
            {
                return NotFound($"Menu with id {id} not found");
            }


            var result = await menuService.UpdateMenu(id, menu);

            return Ok(result);
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            /*if (id <= 0)
            {
                return BadRequest();
            }*/

            var menu = await menuService.GetMenuById(id);
            if (menu == null)
            {
                return NotFound($"Menu with id {id} not found");
            }
            else
            {
                await menuService.DeleteMenu(id);
            }
            return NoContent();
            
        }

        //[Authorize(Roles = "Manager")]
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchUpdate (int id, [FromBody] JsonPatchDocument<Menu>  patch)
        {
            if (id <= 0 || patch == null)

            {
                return BadRequest();
            }

            // check if the menu exists using enter id
            var menuExists = await menuService.GetMenuById(id);
            if(menuExists is null)
            {
                return NotFound($"Menu with id {id} not found");
            }

            Menu partialUpdatedMenue = await menuService.PartialUpdateMenu(id, patch);

            return Ok(partialUpdatedMenue);

        }
    }
}
