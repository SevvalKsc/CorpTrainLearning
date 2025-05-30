using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore; 

namespace CorpTrainLearning.Models
{
    [PrimaryKey(nameof(UserId), nameof(CourseId))]
    public class UserCourse
    {
        [Column(Order = 0)] 
        public int UserId { get; set; }
        
        [Column(Order = 1)]
        public int CourseId { get; set; }
        
        [StringLength(50)]
        public string Progress { get; set; }
        
        [StringLength(50)]
        public string Status { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? CompletionDate { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        
        [ForeignKey("CourseId")]
        [ValidateNever]
        public Course Course { get; set; }
    }
}
