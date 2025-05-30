using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation; 

namespace CorpTrainLearning.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(255)] 
        public string Password { get; set; }
        
        [StringLength(100)]
        public string Company { get; set; }
        
        [StringLength(100)]
        public string Department { get; set; }
        
        [StringLength(100)]
        public string Position { get; set; }
        
        public bool IsLoggedIn { get; set; } = false;
        
        [ValidateNever]
        public ICollection<UserRole> UserRoles { get; set; }

        [ValidateNever]
        public ICollection<UserCourse> UserCourses { get; set; }

        [ValidateNever]
        public ICollection<QuizAttempt> QuizAttempts { get; set; }

        [ValidateNever]
        public Admin Admin { get; set; }
    }
}
