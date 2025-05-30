using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class QuizAttempt
    {
        [Key]
        public int QuizAttemptId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int QuizId { get; set; }
        
        public int Score { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AttemptDate { get; set; }
        
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        
        [ForeignKey("QuizId")]
        [ValidateNever]
        public Quiz Quiz { get; set; }
    }
}