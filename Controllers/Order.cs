using FPTBook_v3.Data;
using Microsoft.AspNetCore.Identity;

namespace FPTBook_v3.Controllers
{
    public class Order
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;


        public Order(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string UserId { get; internal set; }
        public DateTime CreateDate { get; internal set; }
        public int Id { get; internal set; }

        public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders =  _db.Orders.Where(a => a.UserId == userId).ToList();
            return (IEnumerable<Order>)orders;
        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
