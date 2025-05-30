using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        
        public int Duration { get; set; }
        
        [ValidateNever]
        public ICollection<Module> Modules { get; set; }
        
        [ValidateNever]
        public ICollection<Quiz> Quizzes { get; set; }
        
        [ValidateNever]
        public ICollection<UserCourse> UserCourses { get; set; }
    }
}