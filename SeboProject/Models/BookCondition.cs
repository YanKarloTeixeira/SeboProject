using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class BookCondition
    {
        public int BookConditionId { get; set; }
        [Required(ErrorMessage = "You must inform the condition name for catalog.")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = "The condition name must be between 1 and 20 characters.")]
        public string Condition { get; set; }
        public virtual ICollection<Book> Books { get; set; }

    }
}
