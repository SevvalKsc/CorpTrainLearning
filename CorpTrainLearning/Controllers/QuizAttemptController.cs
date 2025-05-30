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
    public class QuizAttemptController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizAttemptController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: QuizAttempt
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.QuizAttempt.Include(q => q.Quiz).Include(q => q.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: QuizAttempt/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizAttempt = await _context.QuizAttempt
                .Include(q => q.Quiz)
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.QuizAttemptId == id);
            if (quizAttempt == null)
            {
                return NotFound();
            }

            return View(quizAttempt);
        }

        // GET: QuizAttempt/Create
        public IActionResult Create()
        {
            ViewData["QuizId"] = new SelectList(_context.Quiz, "QuizId", "QuizId");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email");
            return View();
        }

        // POST: QuizAttempt/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuizAttemptId,UserId,QuizId,Score,AttemptDate")] QuizAttempt quizAttempt)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quizAttempt);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuizId"] = new SelectList(_context.Quiz, "QuizId", "QuizId", quizAttempt.QuizId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", quizAttempt.UserId);
            return View(quizAttempt);
        }

        // GET: QuizAttempt/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizAttempt = await _context.QuizAttempt.FindAsync(id);
            if (quizAttempt == null)
            {
                return NotFound();
            }
            ViewData["QuizId"] = new SelectList(_context.Quiz, "QuizId", "QuizId", quizAttempt.QuizId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", quizAttempt.UserId);
            return View(quizAttempt);
        }

        // POST: QuizAttempt/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuizAttemptId,UserId,QuizId,Score,AttemptDate")] QuizAttempt quizAttempt)
        {
            if (id != quizAttempt.QuizAttemptId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quizAttempt);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuizAttemptExists(quizAttempt.QuizAttemptId))
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
            ViewData["QuizId"] = new SelectList(_context.Quiz, "QuizId", "QuizId", quizAttempt.QuizId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "Email", quizAttempt.UserId);
            return View(quizAttempt);
        }

        // GET: QuizAttempt/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quizAttempt = await _context.QuizAttempt
                .Include(q => q.Quiz)
                .Include(q => q.User)
                .FirstOrDefaultAsync(m => m.QuizAttemptId == id);
            if (quizAttempt == null)
            {
                return NotFound();
            }

            return View(quizAttempt);
        }

        // POST: QuizAttempt/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quizAttempt = await _context.QuizAttempt.FindAsync(id);
            if (quizAttempt != null)
            {
                _context.QuizAttempt.Remove(quizAttempt);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuizAttemptExists(int id)
        {
            return _context.QuizAttempt.Any(e => e.QuizAttemptId == id);
        }
    }
}
