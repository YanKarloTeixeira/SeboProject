using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage = "You must inform the course name for catalog.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "The course name must be between 1 and 100 characters.")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "You must inform the study area that the course is related to.")]
        public int StudyAreaId { get; set; }
        public virtual StudyArea StudyArea { get; set; }

        [Required]
        public int InstitutionId { get; set; }
        public virtual Institution Institution { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
