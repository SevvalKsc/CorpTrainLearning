using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore; 

namespace CorpTrainLearning.Models
{
    [PrimaryKey(nameof(UserId), nameof(RoleId))]
    public class UserRole
    {
        [Column(Order = 0)]
        public int UserId { get; set; }
        
        [Column(Order = 1)]
        public int RoleId { get; set; }
        
        [ForeignKey("UserId")]
        [ValidateNever]
        public User User { get; set; }
        
        [ForeignKey("RoleId")]
        [ValidateNever]
        public Role Role { get; set; }
    }
}