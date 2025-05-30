using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CorpTrainLearning.Data;
using CorpTrainLearning.Models;
using CorpTrainLearning.ViewModels;

namespace CorpTrainLearning.Controllers
{
    public class CourseEnrollmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseEnrollmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseEnrollment/Index
        public async Task<IActionResult> Index()
        {
            var currentUser = await _context.User
                .Include(u => u.UserCourses)
                    .ThenInclude(uc => uc.Course)
                .FirstOrDefaultAsync(u => u.IsLoggedIn);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var allCourses = await _context.Course.ToListAsync();
            
            var enrolledCourseIds = currentUser.UserCourses.Select(uc => uc.CourseId).ToHashSet();
            var enrolledCourses = allCourses
                .Where(c => enrolledCourseIds.Contains(c.CourseId))
                .ToList();
            
            var availableCourses = allCourses
                .Where(c => !enrolledCourseIds.Contains(c.CourseId))
                .ToList();

            var viewModel = new CourseEnrollmentViewModel
            {
                CurrentUser = currentUser,
                EnrolledCourses = enrolledCourses,
                AvailableCourses = availableCourses
            };

            return View(viewModel);
        }

        // POST: CourseEnrollment/EnrollCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnrollCourse(int courseId)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(u => u.IsLoggedIn);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var isEnrolled = await _context.UserCourse
                .AnyAsync(uc => uc.UserId == currentUser.UserId && uc.CourseId == courseId);

            if (!isEnrolled)
            {
                var newUserCourse = new UserCourse
                {
                    UserId = currentUser.UserId,
                    CourseId = courseId,
                    Progress = "Not Started",
                    Status = "Not Started",
                    CompletionDate = null
                };
                _context.UserCourse.Add(newUserCourse);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: CourseEnrollment/DropCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DropCourse(int courseId)
        {
            var currentUser = await _context.User.FirstOrDefaultAsync(u => u.IsLoggedIn);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            
            var userCourseToRemove = await _context.UserCourse
                .FirstOrDefaultAsync(uc => uc.UserId == currentUser.UserId && uc.CourseId == courseId);

            if (userCourseToRemove != null)
            {
                _context.UserCourse.Remove(userCourseToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}