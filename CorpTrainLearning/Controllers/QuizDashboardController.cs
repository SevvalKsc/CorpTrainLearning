using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CorpTrainLearning.Data;
using CorpTrainLearning.Models;
using CorpTrainLearning.ViewModels; // Important: Add this using statement
using System.Collections.Generic;
using System; // For DateTime
using Microsoft.AspNetCore.Http; // For IFormCollection

namespace CorpTrainLearning.Controllers
{
    public class QuizDashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QuizDashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        private async Task<User> GetLoggedInUserAsync()
        {
            return await _context.User
                .Include(u => u.UserCourses)
                    .ThenInclude(uc => uc.Course)
                        .ThenInclude(c => c.Quizzes) 
                .Include(u => u.QuizAttempts) 
                    .ThenInclude(qa => qa.Quiz) 
                        .ThenInclude(q => q.Course) 
                .FirstOrDefaultAsync(u => u.IsLoggedIn);
        }

        // GET: QuizDashboard/Index
        public async Task<IActionResult> Index()
        {
            var currentUser = await GetLoggedInUserAsync();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var viewModel = new QuizDashboardViewModel
            {
                CurrentUser = currentUser,
                QuizzesNotAttempted = new List<QuizInfo>(),
                PreviousQuizAttempts = new List<QuizAttemptInfo>()
            };
            
            var allQuizzesForEnrolledCourses = currentUser.UserCourses
                .SelectMany(uc => uc.Course.Quizzes)
                .Distinct() 
                .ToList();
            
            foreach (var attempt in currentUser.QuizAttempts)
            {
                viewModel.PreviousQuizAttempts.Add(new QuizAttemptInfo
                {
                    QuizAttemptId = attempt.QuizAttemptId,
                    QuizId = attempt.QuizId,
                    CourseTitle = attempt.Quiz?.Course?.Title ?? "N/A",
                    Score = attempt.Score,
                    AttemptDate = attempt.AttemptDate
                });
            }
            
            var attemptedQuizIds = currentUser.QuizAttempts.Select(qa => qa.QuizId).ToHashSet();
            foreach (var quiz in allQuizzesForEnrolledCourses)
            {
                if (!attemptedQuizIds.Contains(quiz.QuizId))
                {
                    viewModel.QuizzesNotAttempted.Add(new QuizInfo
                    {
                        QuizId = quiz.QuizId,
                        CourseTitle = quiz.Course?.Title ?? "N/A",
                        QuizDescription = quiz.Course?.Description ?? "N/A"
                    });
                }
            }

            return View(viewModel);
        }

        // GET: QuizDashboard/TakeQuiz/5
        public async Task<IActionResult> TakeQuiz(int? id)
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

            var quiz = await _context.Quiz
                .Include(q => q.Course)
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(m => m.QuizId == id);

            if (quiz == null)
            {
                return NotFound();
            }
            
            var isEnrolledInCourse = await _context.UserCourse
                .AnyAsync(uc => uc.UserId == currentUser.UserId && uc.CourseId == quiz.CourseId);

            if (!isEnrolledInCourse)
            {
                return RedirectToAction(nameof(Index));
            }

            var viewModel = new TakeQuizViewModel
            {
                QuizId = quiz.QuizId,
                CourseTitle = quiz.Course?.Title ?? "N/A",
                Questions = quiz.Questions.Select(q => new QuestionViewModel
                {
                    QuestionId = q.QuestionId,
                    QuestionText = q.QuestionText,
                    Choices = q.MultipleChoices?.Split('|').ToList() ?? new List<string>()
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: QuizDashboard/SubmitQuiz
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitQuiz(int quizId, IFormCollection form)
        {
            var currentUser = await GetLoggedInUserAsync();
            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var quiz = await _context.Quiz
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.QuizId == quizId);

            if (quiz == null)
            {
                return NotFound();
            }

            int correctAnswersCount = 0;
            foreach (var question in quiz.Questions)
            {
                string userAnswer = form[$"q_{question.QuestionId}"];
                
                if (!string.IsNullOrEmpty(userAnswer) &&
                    userAnswer.Trim().Equals(question.CorrectAnswer.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    correctAnswersCount++;
                }
            }
            
            int totalQuestions = quiz.Questions.Count;
            int score = 0;
            if (totalQuestions > 0)
            {
                score = (int)Math.Round((double)correctAnswersCount / totalQuestions * 100);
            }
            
            var quizAttempt = new QuizAttempt
            {
                UserId = currentUser.UserId,
                QuizId = quizId,
                Score = score,
                AttemptDate = DateTime.Now
            };

            _context.QuizAttempt.Add(quizAttempt);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("AttemptDetails", new { id = quizAttempt.QuizAttemptId });
        }

        // GET: QuizDashboard/AttemptDetails/5
        public async Task<IActionResult> AttemptDetails(int? id)
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

            var quizAttempt = await _context.QuizAttempt
                .Include(qa => qa.User)
                .Include(qa => qa.Quiz)
                    .ThenInclude(q => q.Course)
                .Include(qa => qa.Quiz)
                    .ThenInclude(q => q.Questions) 
                .FirstOrDefaultAsync(qa => qa.QuizAttemptId == id && qa.UserId == currentUser.UserId);

            if (quizAttempt == null)
            {
                return NotFound(); 
            }

            var viewModel = new QuizAttemptDetailsViewModel
            {
                QuizAttemptId = quizAttempt.QuizAttemptId,
                QuizId = quizAttempt.QuizId, 
                CourseTitle = quizAttempt.Quiz?.Course?.Title ?? "N/A",
                Score = quizAttempt.Score,
                AttemptDate = quizAttempt.AttemptDate,
                UserName = quizAttempt.User?.Name ?? "N/A",
                QuestionsReviewed = quizAttempt.Quiz?.Questions.Select(q => new QuestionReviewViewModel
                {
                    QuestionText = q.QuestionText,
                    AllChoices = q.MultipleChoices?.Split('|').ToList() ?? new List<string>(),
                    CorrectAnswer = q.CorrectAnswer
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
