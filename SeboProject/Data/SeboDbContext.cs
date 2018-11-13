using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SeboProject.Models;

namespace SeboProject.Data
{
    public class SeboDbContext : IdentityDbContext
    {
        public SeboDbContext(DbContextOptions<SeboDbContext> options)
            : base(options)
        {
        }
/*         <summary about CASCADE OPTIONS>                                                                                        
                                                                                                                                  
            Behavior Name               Effect on dependent/child in memory             Effect on dependent/child in database     
            -------------               -----------------------------------             -------------------------------------     
                                                                                                                                  
            Cascade                     Entities are deleted                            Entities are deleted                      
            ClientSetNull(Default)      Foreign key properties are set to null	        None                                      
            SetNull                     Foreign key properties are set to null	        Foreign key properties are set to null    
            Restrict                    None                                            None                                      
                                                                                                                                  
                                                                                                                                  
                                                                                                               </end of summary>*/
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }


        public DbSet<SeboProject.Models.Book> Book { get; set; }
        public DbSet<SeboProject.Models.BookCondition> BookCondition { get; set; }
        public DbSet<SeboProject.Models.Course> Course { get; set; }
        public DbSet<SeboProject.Models.Institution> Institution { get; set; }
        public DbSet<SeboProject.Models.InstitutionBranch> InstitutionBranch { get; set; }
        public DbSet<SeboProject.Models.Claim> Claim { get; set; }
        public DbSet<SeboProject.Models.ClaimMediation> ClaimMediation { get; set; }
        public DbSet<SeboProject.Models.Order> Order { get; set; }
        public DbSet<SeboProject.Models.Localization> Localization { get; set; }
        public DbSet<SeboProject.Models.StudyArea> StudyArea { get; set; }
        public DbSet<SeboProject.Models.User> User { get; set; }
        public DbSet<SeboProject.Models.Seller> Seller { get; set; }


    }
}
