using SeboProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeboProject.Utilities
{
    public class OrderingInstitutionBranches
    {

        public static IQueryable<InstitutionBranch> Do(IQueryable<InstitutionBranch> courses, string sortOrder)
        {
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "institution_desc";
            switch (sortOrder.ToLower())
            {
                case "institution_asc":
                    courses = courses.OrderByDescending(c => c.Institution.InstitutionName).ThenBy(c => c.InstitutionBranchName);
                    break;
                case "institution_desc":
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.InstitutionBranchName);
                    break;
                case "branchname_asc":
                    courses = courses.OrderByDescending(c => c.InstitutionBranchName);
                    break;
                case "branchname_desc":
                    courses = courses.OrderBy(c => c.InstitutionBranchName);
                    break;
                default:
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.InstitutionBranchName);
                    break;
            }
            return courses;

        }

        public static string NewOrder(string SortOrder, string ColumnName)
        {
            string NewOrder = null;
            switch (ColumnName.ToLower())
            {

                case "institution":
                    if (SortOrder == "Institution_asc")
                        NewOrder = "Institution_desc";
                    else
                        NewOrder = "Institution_asc";
                    break;
                case "branchname":
                    if (SortOrder == "BranchName_asc")
                        NewOrder = "BranchName_desc";
                    else
                        NewOrder = "BranchName_asc";
                    break;
            }
            return NewOrder;
        }



    }
}
