using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Required for Include and FirstOrDefaultAsync
using CorpTrainLearning.Data; // Required for ApplicationDbContext
using CorpTrainLearning.Models; // Required for User model
using CorpTrainLearning.ViewModels; 

namespace CorpTrainLearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        private async Task<User> GetLoggedInUserAsync()
        {
            return await _context.User
                .Include(u => u.UserRoles) 
                    .ThenInclude(ur => ur.Role) 
                .FirstOrDefaultAsync(u => u.IsLoggedIn);
        }

        // GET: /Home/Index
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetLoggedInUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            bool isAdmin = currentUser.UserRoles.Any(ur => ur.Role?.RoleName == "Admin");

            var viewModel = new HomeViewModel
            {
                CurrentUser = currentUser,
                IsAdmin = isAdmin
            };

            return View(viewModel);
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
