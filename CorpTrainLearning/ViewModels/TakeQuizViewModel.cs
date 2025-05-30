using System.Collections.Generic;
using CorpTrainLearning.Models;

namespace CorpTrainLearning.ViewModels
{
    public class TakeQuizViewModel
    {
        public int QuizId { get; set; }
        public string CourseTitle { get; set; }
        public List<QuestionViewModel> Questions { get; set; }
    }

    public class QuestionViewModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<string> Choices { get; set; }
    }
}