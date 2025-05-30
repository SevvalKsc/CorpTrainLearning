using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
    }
}