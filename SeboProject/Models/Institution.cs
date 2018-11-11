using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class Institution
    {
        public int InstitutionId { get; set; }
        [Required(ErrorMessage = "You must inform the institution name for catalog.")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "The intitution name must be between 1 and 40 characters.")]
        public string InstitutionName { get; set; }
        public virtual ICollection<InstitutionBranch> InstitutionBranches { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
