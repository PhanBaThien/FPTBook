using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FPTBook_v3.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;


        public OrderDetailController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
             IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public IActionResult Show()
        {
            IEnumerable<OrderDetail> ds = _db.OrderDetails.ToList();
            return View(ds);
        }
    }
}
