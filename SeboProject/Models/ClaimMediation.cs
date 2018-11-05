using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Models
{
    public class ClaimMediation
    {
        public int ClaimMediationId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Action { get; set; }

        //Relational 
        [Required]
        //public int AuthorId { get; set; }

        //public virtual Buyer Author { get; set; }

        public int ClaimId { get; set; }
        public Claim Claim { get; set; }

    }
}
