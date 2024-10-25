using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pizza_System.Db;
using Pizza_System.Model;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel;
using System.Web;
using Pizza_System.Controllers;
using Microsoft.AspNetCore.Http;

namespace Pizza_System.Services
{
    public class OrderService : IOrderService
    {
        public AppDbContext context;
        private  readonly UserManager<User> _userManager;
        public IMenuService menuService;
        
        private readonly IHttpContextAccessor accessor;
        
        public OrderController orderController;
        public string CurrentUser { get; set; }
        public OrderService(AppDbContext context, UserManager<User> userManager,
           IMenuService menuService, IHttpContextAccessor _accessor)
        {
            this.context = context;
            _userManager = userManager;
            
            this.menuService = menuService;
           
            accessor = _accessor;
            

        }

        private  async Task<string>  GetUserId()
        {
            var user = await _userManager.FindByNameAsync(accessor.HttpContext.User.Identity.Name);
            return user.Id;
            //string u = user.Id;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {    
            var result =  await context.Orders.Where(u => GetUserId().Result == u.UserId).ToListAsync(); 
            return result;
        }

        public async Task<Order> GetOrderById(int orderId, int menuId/*, string user*/)
        {
            return await context.Orders.FindAsync(orderId, GetUserId().Result, menuId);
        }

        public async Task<Order> CreateOrder(Order order)
        {
            // set current user as foreign key
            order.UserId = GetUserId().Result;

            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order;
        }
        
        // delete an order
        public async Task DeleteOrder(int orderId, int menuId/*, string user*/)
        {
            var order = await context.Orders.FindAsync(orderId, GetUserId().Result, menuId);
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }

        // update an order
        public async Task<Order> UpdateOrder(int orderId, int menuId, /*string user,*/ Order newUpdateOrder)
        {
            var menu = await menuService.GetMenuById(menuId);
            // retrieve the menu using the id
            var oldOrder = await context.Orders.FindAsync(orderId, GetUserId().Result, menuId);

            // if deliveryOption was Collection and it is updated to Delivery, clear the address values
            if (newUpdateOrder.DeliveryOption.Equals("Delivery")) {
                oldOrder.DeliveryOption = newUpdateOrder.DeliveryOption;
                oldOrder.Address1 = newUpdateOrder.Address1;
                oldOrder.Address2 = newUpdateOrder.Address2;
                oldOrder.Street = newUpdateOrder.Street;
                oldOrder.PostCode = newUpdateOrder.PostCode;
                oldOrder.PhoneNumber = newUpdateOrder.PhoneNumber;
                oldOrder.Quantity = newUpdateOrder.Quantity;
                oldOrder.Total = newUpdateOrder.Quantity * menu.Price;
            }

            else
            {
                oldOrder.DeliveryOption = newUpdateOrder.DeliveryOption;
                oldOrder.Address1 = "";
                oldOrder.Address2 = "";
                oldOrder.Street = "";
                oldOrder.PostCode = "";
                oldOrder.PhoneNumber = newUpdateOrder.PhoneNumber;
                oldOrder.Quantity = newUpdateOrder.Quantity;
                oldOrder.Total = newUpdateOrder.Quantity * menu.Price;
            }

          
            await context.SaveChangesAsync();
            return oldOrder;

        }
    }
}
