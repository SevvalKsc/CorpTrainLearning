using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CorpTrainLearning.Data;
using CorpTrainLearning.Models;
using CorpTrainLearning.ViewModels;
using System.Collections.Generic;

namespace CorpTrainLearning.Controllers
{
    public class ModuleDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModuleDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        private async Task<User> GetLoggedInUserAsync()
        {
            return await _context.User
                .Include(u => u.UserCourses)
                    .ThenInclude(uc => uc.Course)
                        .ThenInclude(c => c.Modules)
                .FirstOrDefaultAsync(u => u.IsLoggedIn);
        }

        // GET: ModuleDashboard/Index
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetLoggedInUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new ModuleDashboardViewModel
            {
                CurrentUser = currentUser,
                EnrolledCourses = new List<CourseWithModulesViewModel>()
            };
            
            foreach (var userCourse in currentUser.UserCourses)
            {
                var course = userCourse.Course;
                if (course != null)
                {
                    var courseViewModel = new CourseWithModulesViewModel
                    {
                        CourseId = course.CourseId,
                        CourseTitle = course.Title,
                        Modules = new List<ModuleViewModel>()
                    };

                    foreach (var module in course.Modules.OrderBy(m => m.ModuleId))
                    {
                        courseViewModel.Modules.Add(new ModuleViewModel
                        {
                            ModuleId = module.ModuleId,
                            ModuleType = module.Type,
                            ContentSnippet = module.ContentDetails != null && module.ContentDetails.Length > 50
                                ? module.ContentDetails.Substring(0, 50) + "..."
                                : module.ContentDetails ?? "No content details"
                        });
                    }
                    viewModel.EnrolledCourses.Add(courseViewModel);
                }
            }

            return View(viewModel);
        }

        // GET: ModuleDashboard/ViewModuleContent/5
        public async Task<IActionResult> ViewModuleContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var currentUser = await GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var module = await _context.Module
                .Include(m => m.Course)
                .FirstOrDefaultAsync(m => m.ModuleId == id);

            if (module == null)
            {
                return NotFound();
            }

            var isUserEnrolled = await _context.UserCourse
                .AnyAsync(uc => uc.UserId == currentUser.UserId && uc.CourseId == module.CourseId);

            if (!isUserEnrolled)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new FullModuleContentViewModel
            {
                ModuleId = module.ModuleId,
                ModuleType = module.Type,
                CourseTitle = module.Course?.Title ?? "N/A",
                FullContent = module.ContentDetails ?? "No content available."
            };

            return View(viewModel);
        }
    }
}
