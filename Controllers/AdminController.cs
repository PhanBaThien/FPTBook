using FPTBook_v3.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPTBook_v3.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<ApplicationUser> _rolemanager;
        public AdminController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> ShowUser()
        {   
            var user = await (from users in _db.Users
                              join UserRole in _db.UserRoles
                              on users.Id equals UserRole.UserId
                              join role in _db.Roles
                              on UserRole.RoleId equals role.Id
                              where role.Name == "User"
                              select users).ToListAsync();
            return View(user);
        }

        public async Task<IActionResult> ShowOwner()
        {
            var owner = await (from users in _db.Users
                              join UserRole in _db.UserRoles
                              on users.Id equals UserRole.UserId
                              join role in _db.Roles
                              on UserRole.RoleId equals role.Id
                              where role.Name == "Owner"
                              select users).ToListAsync();
            return View(owner);
        }

        public IActionResult RequestCategory()
        {
            var category = _db.Categorys.Where(c => c.cate_Status == "processing").ToList();
            return View(category);
        }

        public IActionResult Approval(int id)
        {
            Category category = _db.Categorys.Find(id);
            if (category == null)
            {
                return RedirectToAction("RequestCategory");
            }
            else
            {
                category.cate_Status = "processed";
                _db.Categorys.Update(category);
                _db.SaveChanges();
                return RedirectToAction("RequestCategory");
            }
            
        }


        public IActionResult Reject(int id)
        {
            Category category = _db.Categorys.Find(id);
            if (category == null)
            {
                return RedirectToAction("RequestCategory");
            }
            else
            {
                _db.Categorys.Remove(category);
                _db.SaveChanges();
                return RedirectToAction("RequestCategory");
            }
        }
    }
}
