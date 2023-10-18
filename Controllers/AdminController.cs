using FPTBook_v3.Constants;
using FPTBook_v3.Models;
using FPTBook_v3.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FPTBook_v3.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;
        private readonly IFileService _fileService;
        public AdminController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            IFileService fileService)
        {
            _db = db;
            _userManager = userManager;
            _fileService = fileService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Admin/ShowUser")]
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

        [HttpGet]
        [Route("Admin/EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return RedirectToAction("ShowUser");
            }

            return View(user);
        }

        [HttpPost]
        [Route("Admin/EditUser/{id}")]
        public async Task<IActionResult> EditUser(string id, ApplicationUser editedUser)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
            {
                return RedirectToAction("ShowUser");
            }

            existingUser.User_Name = editedUser.User_Name;
            existingUser.Email = editedUser.Email;
            existingUser.PhoneNumber = editedUser.PhoneNumber;

            if (editedUser.User_Img != null)
            {
                var result = _fileService.SaveImage(editedUser.User_Img);
                if (result.Item1 == 1)
                {
                    var oldImage = existingUser.User_Img;
                    existingUser.User_Img = result.Item2;

                    if (oldImage != null)
                    {
                        var deleteResult = _fileService.Delete(oldImage);
                        
                    }
                }
            }

            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                TempData["Fail"] = "EditUser Fail!";
                return RedirectToAction("EditUser", new { id = id });
            }

            return RedirectToAction("ShowUser");
        }


        [Route("Admin/ShowOwner")]
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


        [Route("Admin/RequestCategory")]
        public IActionResult RequestCategory()
        {
            var category = _db.Categorys.Where(c => c.cate_Status == "processing").ToList();
            return View(category);
        }

        [Route("Admin/RequestCategory/Approval")]
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


        [Route("Admin/RequestCategory/Reject")]
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

        [Route("Admin/RegisterOwner")]
        public async Task<IActionResult> RegisterOwner()
        {
            return View();
        }

        [HttpPost]
        [Route("Admin/RegisterOwner")]
        public async Task<IActionResult> RegisterOwner(Owner owners)
        {
            if (ModelState.IsValid)
            {
                var owner = new ApplicationUser
                {
                    UserName = owners.Email,
                    User_Name = owners.Name,
                    Email = owners.Email,
                    PhoneNumber = owners.Phone,
                    

                };

                if (owners.Img != null)
                {
                    var resultt = _fileService.SaveImage(owners.Img);
                    if (resultt.Item1 == 1)
                    {
                        var oldImage = owner.User_Img;
                        owner.User_Img = resultt.Item2;

                        
                        if (oldImage == null)
                        {

                        }
                        else
                        {
                            var delete = _fileService.Delete(oldImage);
                        }

                    }
                }

                var result = await _userManager.CreateAsync(owner, owners.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(owner, Role.Owner.ToString());
                    return RedirectToAction("ShowOwner");
                }
                else
                {
                    TempData["Fail"] = "RegisterOwner Fail!";
                    return RedirectToAction("RegisterOwner");
                }
            }
            return RedirectToAction("RegisterOwner");

        }

        [HttpGet]
        [Route("Admin/EditOwner/{id}")]
        public async Task<IActionResult> EditOwner(string id)
        {
            var owner = await _userManager.FindByIdAsync(id);

            if (owner == null)
            {
                return RedirectToAction("ShowOwner");
            }

            return View(owner);
        }

        [HttpPost]
        [Route("Admin/EditOwner/{id}")]
        public async Task<IActionResult> EditOwner(string id, ApplicationUser editedOwner)
        {
            var existingOwner = await _userManager.FindByIdAsync(id);

            if (existingOwner == null)
            {
                return RedirectToAction("ShowOwner");
            }

            existingOwner.User_Name = editedOwner.User_Name;
            existingOwner.Email = editedOwner.Email;
            existingOwner.PhoneNumber = editedOwner.PhoneNumber;

            if (editedOwner.User_Img != null)
            {
                var result = _fileService.SaveImage(editedOwner.User_Img);
                if (result.Item1 == 1)
                {
                    var oldImage = existingOwner.User_Img;
                    existingOwner.User_Img = result.Item2;

                    if (oldImage != null)
                    {
                        var deleteResult = _fileService.Delete(oldImage);
                        
                    }
                }
            }

            // Khi cập nhật thông tin chủ sở hữu, hãy sử dụng `UpdateAsync` từ `_userManager`
            var updateResult = await _userManager.UpdateAsync(existingOwner);
            if (!updateResult.Succeeded)
            {
                // Xử lý lỗi khi cập nhật không thành công
                TempData["Fail"] = "EditOwner Fail!";
                return RedirectToAction("EditOwner", new { id = id });
            }

            return RedirectToAction("ShowOwner");
        }
       

    }
}
