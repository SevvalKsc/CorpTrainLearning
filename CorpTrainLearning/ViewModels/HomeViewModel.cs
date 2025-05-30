using CorpTrainLearning.Models;

namespace CorpTrainLearning.ViewModels
{
    public class HomeViewModel
    {
        public User CurrentUser { get; set; }
        public bool IsAdmin { get; set; } 
    }
}