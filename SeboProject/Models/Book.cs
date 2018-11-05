using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class Book
    {
        public int BookId { get; set; }


        [Required(ErrorMessage = "You must inform the book's title.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "The book's title size must be between 1 and 100 characters.")]
        public string Title { get; set; } //

        [StringLength(1000, MinimumLength = 1, ErrorMessage = "The book's description size must be between 1 and 1000 characters.")]
        public string Description { get; set; } //

        [Required(ErrorMessage = "You must inform the book's ISBN.")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "The book's ISBN size must be between 10 and 13 characters.")]
        [RegularExpression("^[0-9]$")]
        public string ISBN { get; set; }//


        [Required(ErrorMessage = "You must inform the book's publisher.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "The book's publisher size must be between 5 and 40 characters.")]
        public string Publisher { get; set; }//



        [Range(0, 99, ErrorMessage = "The book edition must be between 1 and 99.")]
        public int Edition { get; set; } = 0;//


        [Required(ErrorMessage = "You must inform the quantity.")]
        [Range(1,32000,ErrorMessage ="The informed quantity must be at least equals a 1.")]
        public int Quantity { get; set; } = 1;//

        [Range(0, 99999, ErrorMessage = "The informed price is too high."), DataType(DataType.Currency)]
        public double Price { get; set; }//

        public int Visualizations { get; set; } = 0; // How many visualizations
        public int QuantitySold { get; set; } = 0; //Just incase the ad has been created having more than 1 exemplar.
        public bool Blocked { get; set; }// If it is true the ad is not active for selling

        public bool IsWaitList { get; set; } = false; // Makes the book registration a inform of demmand for potential sellers
        public DateTime CreationDate { get; set; }
        
        // 
        //Relational properties
        // 
        [Required(ErrorMessage = "You must inform the book's condition")]
        public int BookConditionId { get; set; } // Informs whether the book is new, like new, very good, acceptable, etc.
        public virtual BookCondition BookCondition { get; set; }

        [Required]
        public int StudyAreaId { get; set; } // Informs which area of study the subject belongs  Engeneering, Math, IT, Bio, Humans, Law, etc
        public virtual StudyArea StudyArea { get; set; }

        public int SellerId { get; set; }
        public virtual Seller Seller { get; set; }

        public virtual ICollection<Order>   Orders { get; set; }

    }
}
