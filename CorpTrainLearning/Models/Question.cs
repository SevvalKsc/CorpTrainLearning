using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        
        [Required]
        public int QuizId { get; set; }
        
        [Required]
        public string QuestionText { get; set; }
        
        public string MultipleChoices { get; set; } 
        
        [Required]
        public string CorrectAnswer { get; set; }
        
        [ForeignKey("QuizId")]
        [ValidateNever]
        public Quiz Quiz { get; set; }
    }
}