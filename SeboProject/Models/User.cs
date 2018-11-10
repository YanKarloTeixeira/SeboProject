using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class User
    {
        public int UserId { get; set; }//

        [Required(ErrorMessage = "You must define a user name.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Your user name size must be between 5 and 100 characters.")]
        public string UserName { get; set; }//

        [Required(ErrorMessage = "You must informe your first name.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Your first name size must be between 1 and 30 characters.")]
        public string FirstName { get; set; } //

        [StringLength(30, MinimumLength = 1, ErrorMessage = "Your middle name size must be between 1 and 30 characters.")]
        public string MiddleName { get; set; }//

        [Required(ErrorMessage = "You must inform your last name.")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Your last name size must be between 1 and 30 characters.")]
        public string LastName { get; set; }//

        [Required(ErrorMessage = "You must choose a role.")]
        public int UserType { get; set; }// Student or graduated or external
        [StringLength(90)]
        public string Address { get; set; }//

        [RegularExpression("^[0-9]{6}$")]
        public string Number { get; set; }//
        public string AddressComplement { get; set; }//

        [Required(ErrorMessage = "You must inform your age.")]
        [Range(15, 90)]
        public int Age { get; set; }//
        [Required]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Your email size must be between 8 and 150 characters.")]
        [EmailAddress]
        public string Email { get; set; }//
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }//
        [Required]
        public double Creditcard { get; set; }//

        [Required]
        [StringLength(22, MinimumLength = 1, ErrorMessage = "The size of your name on the credicart  must be at maximum of  22 characters.")]
        public double CreditcardName { get; set; }//


        // Relational Properties
        public int LocalizationId { get; set; }
        public virtual Localization Localization { get; set; }


        public int InstitutionBranchId { get; set; }
        public virtual InstitutionBranch InstitutionBranch { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Claim> Claims { get; set; }
        



    }
}
