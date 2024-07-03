using EmployeesManagment.Data;
using EmployeesManagment.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using EmployeesManagment.Models;

namespace EmployeesManagment.Controllers
{
    public class RolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public RolesController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
          
            return View(roles);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
        
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string Id , RolesViewModel model)
        {
            var ifexisit = await _roleManager.RoleExistsAsync(model.RoleName);
            if (ifexisit)
            {
                return View(model);
            }

            IdentityRole role = new IdentityRole();
            role.Name = model.RoleName;
            role.NormalizedName = model.RoleName;
            var resault = await _roleManager.CreateAsync(role);
            if (resault.Succeeded)
            {
               return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
          
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {
            var Role = new RolesViewModel();
            var model = await _roleManager.FindByIdAsync(Id);
            Role.RoleName = model.Name;
            Role.RoleId = model.Id;
                return View(Role);
          

        }
        [HttpPost]
        public async Task<IActionResult> Edit(string Id,RolesViewModel model)
        {
            var ifexisit = await _roleManager.RoleExistsAsync(model.RoleName);
            if (ifexisit)
            {
                return View(model);
            }
            var result = await _roleManager.FindByIdAsync(Id);
            result.Name = model.RoleName;
            result.NormalizedName = model.RoleName;

            var r = await _roleManager.UpdateAsync(result);

            if (r.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }

            


        }
    }
}
