using System.Collections.Generic;
using CorpTrainLearning.Models;

namespace CorpTrainLearning.ViewModels
{
    public class QuizAttemptDetailsViewModel
    {
        public int QuizAttemptId { get; set; }
        public int QuizId { get; set; }
        public string CourseTitle { get; set; }
        public int Score { get; set; }
        public System.DateTime AttemptDate { get; set; }
        public string UserName { get; set; }

        public List<QuestionReviewViewModel> QuestionsReviewed { get; set; }
    }

    public class QuestionReviewViewModel
    {
        public string QuestionText { get; set; }
        public List<string> AllChoices { get; set; }
        public string CorrectAnswer { get; set; }
    }
}