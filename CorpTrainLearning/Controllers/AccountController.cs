using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // For FirstOrDefaultAsync

using CorpTrainLearning.Models;
using CorpTrainLearning.Data;

namespace CorpTrainLearning.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError(string.Empty, "Email and password are required.");
                return View();
            }
            
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            
            if (user == null || user.Password != password)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
            
            user.IsLoggedIn = true;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User model, string confirmPassword)
        {
            if (model.Password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Password and confirmation password do not match.");
            }

            if (ModelState.IsValid)
            {
                if (await _context.User.AnyAsync(u => u.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "This email address is already registered.");
                    return View(model);
                }

                model.IsLoggedIn = false;
                _context.User.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login");
            }
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var loggedInUser = await _context.User.FirstOrDefaultAsync(u => u.IsLoggedIn);

            if (loggedInUser != null)
            {
                loggedInUser.IsLoggedIn = false;
                _context.User.Update(loggedInUser);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index", "Home");
        }
    }
}
