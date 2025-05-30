using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CorpTrainLearning.Data;
using CorpTrainLearning.Models;

namespace CorpTrainLearning.Controllers
{
    public class UserCourseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCourseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserCourse
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserCourse.Include(u => u.Course).Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserCourse/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourse
                .Include(u => u.Course)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // GET: UserCourse/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "Title");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: UserCourse/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,CourseId,Progress,Status,CompletionDate")] UserCourse userCourse)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCourse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "Title", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", userCourse.UserId);
            return View(userCourse);
        }

        // GET: UserCourse/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourse.FindAsync(id);
            if (userCourse == null)
            {
                return NotFound();
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "Title", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", userCourse.UserId);
            return View(userCourse);
        }

        // POST: UserCourse/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,CourseId,Progress,Status,CompletionDate")] UserCourse userCourse)
        {
            if (id != userCourse.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCourseExists(userCourse.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "Title", userCourse.CourseId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", userCourse.UserId);
            return View(userCourse);
        }

        // GET: UserCourse/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCourse = await _context.UserCourse
                .Include(u => u.Course)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userCourse == null)
            {
                return NotFound();
            }

            return View(userCourse);
        }

        // POST: UserCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCourse = await _context.UserCourse.FindAsync(id);
            if (userCourse != null)
            {
                _context.UserCourse.Remove(userCourse);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCourseExists(int id)
        {
            return _context.UserCourse.Any(e => e.UserId == id);
        }
    }
}
