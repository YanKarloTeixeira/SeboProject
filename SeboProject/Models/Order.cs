using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }
    
        public int Quantity { get; set; } = 1;
        public double Price { get; set; }
        public string PaymentForm { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime PaymentDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public DateTime CancelationDate { get; set; }
        public DateTime CanfirmationDate { get; set; }
        public string Status { get; set; }
        public bool IsConfirmedByBuyer { get; set; }

        // Relational Properties
        //public int UserId { get; set; }
        //public virtual User User { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
        
        public  int SellerId { get; set; }
        public virtual int Seller { get; set; }

        public virtual ICollection<Claim> Claims { get; set; }


    }
}
