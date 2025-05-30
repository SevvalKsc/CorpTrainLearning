using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class Quiz
    {
        [Key]
        public int QuizId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        [ForeignKey("CourseId")]
        [ValidateNever]
        public Course Course { get; set; }
        
        [ValidateNever]
        public ICollection<Question> Questions { get; set; }
        
        [ValidateNever]
        public ICollection<QuizAttempt> QuizAttempts { get; set; }
    }
}