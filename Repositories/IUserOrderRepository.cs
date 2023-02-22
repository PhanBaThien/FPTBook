using FPTBook_v3.Models;

namespace FPTBook_v3.Repositories
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> UserOrders();
    }
}
