using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pizza_System.Db;
using Pizza_System.Model;
using Pizza_System.Services;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using System.Net.WebSockets;
using System.Net;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pizza_System.Controllers
{
    [Route("v1/api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext context;
        public UserManager<User> userManager;
        public IMenuService menuService;
        public IOrderService orderService;
       
        public OrderController(AppDbContext context,
            UserManager<User> userManager,
            IMenuService menuService, 
            IOrderService orderService )
        {
            this.context = context;
            this.userManager = userManager;
            this.menuService = menuService;
            this.orderService = orderService;
            //this.httpContextAccessor = httpContextAccessor;
        }
        
        [Authorize(Roles = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var user = await userManager.FindByNameAsync(User.Identity.Name);

            // set user`s id as foreingk key in orders` table
            var result = await orderService.GetAllOrdersAsync();
            return Ok(result);
            //return orderService.GetAllOrdersAsync();
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("{orderId:int:min(1)}/{menuId:int:min(1)}")]
        public async Task<IActionResult> GetOrderById(int orderId, int menuId)
        {
            
            var anOrder = await orderService.GetOrderById(orderId, menuId);
            
            if (anOrder is null )
            {
                return NotFound($"Order with {orderId} & {menuId} doesnnt exsits or you dont have permission");
            }
            return Ok(anOrder);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order)
        {

            // get the menu using the menuId that is entered as foreign key
            Menu menu = await menuService.GetMenuById(order.MenuId);

            if(menu is null)
            {
                return NotFound($"Menu with Id {order.MenuId} does not exists");
            }

            /* calculate the total of the order by multiplying the price 
             * of the menu and quantity
            */
            order.Total = menu.Price * order.Quantity;

            var newOrder = await orderService.CreateOrder(order);

            return new ObjectResult(newOrder) { StatusCode = StatusCodes.Status201Created };
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("{orderId}/{menuId}")] 
        public async Task<IActionResult> DeleteOrder(int orderId, int menuId) 
        {
            
            if (orderId <= 0 || menuId <= 0)
            {
                return BadRequest($"{orderId} and {menuId} must valid");

            }
            
            // get the 
            var order = await orderService.GetOrderById(orderId, menuId);

            if (order is null)
            {
                return NotFound($"order with {orderId} and {menuId} does not exists");
            }

            else
            {
                await orderService.DeleteOrder(orderId, menuId);
            }


            return NoContent();        
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("{orderId:int:min(1)}/{menuId:int:min(1)}")]
        public async Task<IActionResult> UpdateOrder(int orderId , int menuId, Order updateOrder)
        {
            if (orderId <= 0 || menuId <= 0)
            {
                return BadRequest($"{orderId} and {menuId} must valid");

            }

            // check if the order exists with orderId, menuId
            var order = await orderService.GetOrderById(orderId, menuId);

            if (order is null)
            {
                return NotFound($"order with {orderId} and {menuId} does not exists");
            }

             var theNewUpdatedOrder = await orderService.UpdateOrder(orderId, menuId, /*user.Id,*/ updateOrder);
     
            return Ok(theNewUpdatedOrder);
        }

    }
}
