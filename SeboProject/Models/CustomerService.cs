using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
   
    public class CustomerService
    { 
        [Key]
        public int CustomerId { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "You must enter your name and last name")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "You must enter a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "You must enter your postal code")]
        public string PostCode { get; set; }
        [Required(ErrorMessage = "You must enter your code number")]
        [Display(Name = "Code Number")]
        public string CodeNumber { get; set; }
        public string Message { get; set; }
    }
}
