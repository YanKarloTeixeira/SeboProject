using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class Localization
    {
        public int LocalizationId { get; set; }
        [Required(ErrorMessage = "You must informe the 3 first letters of place's postal code.")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "You must inform 3 characters.")]
        public string PostalCode { get; set; }


        [Required(ErrorMessage = "You must define the place name.")]
        [StringLength(170, MinimumLength = 5, ErrorMessage = "Your place name size must be between 1 and 170 characters.")]
        public string PlaceName { get; set; }


        [Required(ErrorMessage = "You must inform the province's name.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "The province's name size must be between 5 and 30 characters.")]
        public string Province { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
