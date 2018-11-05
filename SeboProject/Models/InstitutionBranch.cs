using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class InstitutionBranch
    {
        public int InstitutionBranchId { get; set; }
        [Required(ErrorMessage = "You must inform the branch name for catalog.")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "The branch name must be between 1 and 40 characters.")]
        public string InstitutionBranchName { get; set; }
        [Required]
        public int InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
