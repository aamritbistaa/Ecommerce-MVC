using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _roleManager=roleManager;
            _userManager=userManager;
        }

        

        [HttpGet]
        public IActionResult GetAll()
        {
            //var userList = _db.ApplicationUser.Include(c=>c.CompanyUser).ToList();
            //var userRole = _db.UserRoles.ToList();
            //var roles = _db.Roles.ToList();
            //foreach (var user in userList)
            //{
            //    //find the assocated roleID
            //    var roleId = userRole.FirstOrDefault(u=>u.UserId==user.Id).RoleId;
            //    user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
            //    if (user.CompanyUser == null)
            //    {
            //        user.CompanyUser = new CompanyUser()
            //        {   
            //            Name = ""
            //        };
            //    }
            //}
            var userList = _db.ApplicationUser
                .Include(c => c.CompanyUser)
                .ToList();

            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                // Find the associated roleId
                var userRoleIdEntry = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (userRoleIdEntry != null)
                {
                    var roleId = userRoleIdEntry.RoleId;

                    // Find the role name based on roleId
                    var roleName = roles.FirstOrDefault(r => r.Id == roleId)?.Name;

                    // Set the role name for the user
                    user.Role = roleName;
                }
                else
                {
                    // Handle the case where no user role is found
                    // This could be logging the issue or providing feedback to the user
                    // In this example, we set an empty string as the role
                    user.Role = "";
                }

                // If CompanyUser is null, initialize it with an empty CompanyUser object
                if (user.CompanyUser == null)
                {
                    user.CompanyUser = new CompanyUser()
                    {
                        Name = ""
                    };
                }
            }

            return View(userList);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeToSeller(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            if(!await _userManager.IsInRoleAsync(user, SD.Role_Admin))
            {


                var isInSellerRole = await _userManager.IsInRoleAsync(user, SD.Role_Seller);
                if (isInSellerRole)
                {
                    return BadRequest("User is Seller.");
                }

                var existingRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existingRoles);

                var result = await _userManager.AddToRoleAsync(user, SD.Role_Seller);
                if (!result.Succeeded)
                {
                    return BadRequest("Failed to add user to the Seller role.");
                }
            }

            return RedirectToAction("GetAll");
        }
        [HttpPost]
        public async Task<IActionResult> ChangeToUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound();
            }
            if (!await _userManager.IsInRoleAsync(user, SD.Role_Admin))
            {

                var isInSellerRole = await _userManager.IsInRoleAsync(user, SD.Role_User);
            if (isInSellerRole)
            {
                return BadRequest("User is already in User Role.");
            }

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            var result = await _userManager.AddToRoleAsync(user, SD.Role_User);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to add user to the User role.");
                }
            }

            return RedirectToAction("GetAll");
        }

    }
}
