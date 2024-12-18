
using DevSkill.Inventory.Infrastructure.Identity;
using DevSkill.Inventory.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Web.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public MemberController(RoleManager<ApplicationRole> roleManager,
               UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

      
        public async Task<IActionResult> UsersShow()
        {
            var users = await _userManager.Users.ToListAsync();

            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Await roles for each user
                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    Email = user.Email,
                    Roles = string.Join(", ", roles) // Join the roles into a comma-separated string
                });
            }

            return View(userViewModels);
        }


		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteUser(Guid id)
		{
			var user = await _userManager.FindByIdAsync(id.ToString());
			if (user == null)
			{
				TempData["ErrorMessage"] = "User not found!";
				return RedirectToAction("UsersShow");
			}

			// Step 1: Remove user from all roles
			var roles = await _userManager.GetRolesAsync(user);
			var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, roles);

			// Check if the removal was successful
			if (!removeRolesResult.Succeeded)
			{
				TempData["ErrorMessage"] = "Failed to remove user from roles.";
				return RedirectToAction("UsersShow");
			}

			// Step 2: Delete the user
			var result = await _userManager.DeleteAsync(user);
			if (result.Succeeded)
			{
				TempData["SuccessMessage"] = "User deleted successfully!";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to delete user.";
			}

			return RedirectToAction("UsersShow");
		}



		public IActionResult RolesShow()
        {
            var roles = _roleManager.Roles
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    NormalizedName = r.NormalizedName
                }).ToList();

            return View(roles);
        }



        [Authorize(Roles = "Admin")]
        public IActionResult CreateRole()
        {
            var model = new RoleCreateModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    NormalizedName = model.Name.ToUpper(),
                    Name = model.Name,
                    ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
                });

                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role created successfully!";
                    return RedirectToAction("RolesShow");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found!";
                return RedirectToAction("RolesShow");
            }

            var model = new RoleEditModel
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName
            };

            return View(model);
        }

        // Edit Role POST Method
        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditRole(RoleEditModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id.ToString());
                if (role == null)
                {
                    TempData["ErrorMessage"] = "Role not found!";
                    return RedirectToAction("RolesShow");
                }

                role.Name = model.Name;
                role.NormalizedName = model.Name.ToUpper(); // Ensure the normalized name is updated

                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role updated successfully!";
                    return RedirectToAction("RolesShow");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }

        // Delete Role Method
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                TempData["ErrorMessage"] = "Role not found!";
                return RedirectToAction("RolesShow");
            }

            // Optional: You can add checks here to ensure the role isn't being used by any users
            var usersWithRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if (usersWithRole.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete a role that is assigned to users!";
                return RedirectToAction("RolesShow");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role deleted successfully!";
                return RedirectToAction("RolesShow");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    TempData["ErrorMessage"] = error.Description;
                }
                return RedirectToAction("RolesShow");
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ChangeRole()
        {
            var model = new RoleChangeModel();
            LoadValues(model);
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeRole(RoleChangeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId.ToString());
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found.");
                    LoadValues(model);
                    return View(model);
                }

                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles);

                var newRole = await _roleManager.FindByIdAsync(model.RoleId.ToString());
                if (newRole == null)
                {
                    ModelState.AddModelError("", "Role not found.");
                    LoadValues(model);
                    return View(model);
                }

                var result = await _userManager.AddToRoleAsync(user, newRole.Name);
                if (result.Succeeded)
                {
                    TempData["SuccessMessage"] = "Role updated successfully!";
                    return RedirectToAction("UsersShow"); // Redirect to the Index action
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            LoadValues(model);
            return View(model);
        }


        private void LoadValues(RoleChangeModel model)
        {
            var users = from c in _userManager.Users.ToList() select c;
            var roles = from c in _roleManager.Roles.ToList() select c;

            model.UserId = users.First().Id;
            model.RoleId = roles.First().Id;

            model.Users = new SelectList(users, "Id", "UserName");
            model.Roles = new SelectList(roles, "Id", "Name");
        }


    }
}
