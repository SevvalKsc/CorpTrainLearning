using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CorpTrainLearning.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        
        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }
        
        [ValidateNever]
        public ICollection<UserRole> UserRoles { get; set; }
    }
}