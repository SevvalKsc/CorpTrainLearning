using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class Module
    {
        [Key]
        public int ModuleId { get; set; }
        
        [Required]
        public int CourseId { get; set; }
        
        [StringLength(50)]
        public string Type { get; set; }
        
        public string ContentDetails { get; set; }
        
        [ForeignKey("CourseId")]
        [ValidateNever]
        public Course Course { get; set; }
    }
}