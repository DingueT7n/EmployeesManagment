using EmployeesManagment.Data;
using EmployeesManagment.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeesManagment.Models;

namespace EmployeesManagment.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        

        public UsersController(UserManager<ApplicationUser> userManager,
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
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
        [HttpGet]
        public IActionResult Create() 
        { 
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel userDto)
        {
            //ApplicationUser user = new ApplicationUser();
            //user.UserName = User.UserName;
            //user.NormalizedUserName =  user.UserName;
            //user.Email = User.Email;
            //user.EmailConfirmed = true;
            //user.NormalizedEmail = user.Email;
            //user.PhoneNumber = User.PhoneNumber;
            //user.PhoneNumberConfirmed = true;

            var user = new ApplicationUser
            {
                UserName = userDto.UserName,
                NormalizedUserName = userDto.UserName,
                Email = userDto.Email,
                NormalizedEmail = userDto.Email,
                EmailConfirmed = true,
                PhoneNumber = userDto.PhoneNumber,
                PhoneNumberConfirmed = true
            };

           var result= await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Return the errors
                return View(userDto);
            }

           
        }
    }
}
