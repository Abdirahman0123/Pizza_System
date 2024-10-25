using Pizza_System.Model;
using System.Security.Claims;

namespace Pizza_System.Services
{
    public interface IOrderService
    {
        //public IQueryable<Order> GetAllOrders();
        public Task<List<Order>> GetAllOrdersAsync();
        public Task<Order> GetOrderById(int orderId, int menuId/*, string user*/);

        public Task<Order> CreateOrder(Order order);
        public  Task DeleteOrder(int orderId, int menuId);

        public Task<Order> UpdateOrder(int orderId, int menuId,/*, string user,*/ Order newOrder);

        
    }
}
