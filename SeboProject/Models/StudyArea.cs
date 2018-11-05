using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class StudyArea
    {
        public int StudyAreaId { get; set; }
        [Required(ErrorMessage = "You must inform the study area name for catalog.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "The study area name must be between 1 and 30 characters.")]
        public string StudyAreaName { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
