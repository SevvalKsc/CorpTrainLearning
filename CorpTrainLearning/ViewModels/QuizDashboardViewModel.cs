using System.Collections.Generic;
using CorpTrainLearning.Models;

namespace CorpTrainLearning.ViewModels
{
    public class QuizDashboardViewModel
    {
        public User CurrentUser { get; set; }
        public List<QuizInfo> QuizzesNotAttempted { get; set; }
        public List<QuizAttemptInfo> PreviousQuizAttempts { get; set; }
    }

    public class QuizInfo
    {
        public int QuizId { get; set; }
        public string CourseTitle { get; set; }
        public string QuizDescription { get; set; } 
    }
    
    public class QuizAttemptInfo
    {
        public int QuizAttemptId { get; set; }
        public int QuizId { get; set; }
        public string CourseTitle { get; set; }
        public int Score { get; set; }
        public System.DateTime AttemptDate { get; set; }
    }
}