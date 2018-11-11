using SeboProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeboProject.Utilities
{
    public class OrderingPostalCodes
    {
        public static IQueryable<Localization> Do(IQueryable<Localization> PostalCodes, string sortOrder)
        {
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "postalCode_desc";
            switch (sortOrder.ToLower())
            {
                case "postalCode_asc":
                    PostalCodes = PostalCodes.OrderByDescending(p => p.PostalCode);
                    break;
                case "postalCode_desc":
                    PostalCodes = PostalCodes.OrderBy(p => p.PostalCode);
                    break;
                case "province_asc":
                    PostalCodes = PostalCodes.OrderByDescending(p => p.Province).ThenBy(p => p.PlaceName);
                    break;
                case "province_desc":
                    PostalCodes = PostalCodes.OrderBy(p => p.Province).ThenBy(p => p.PlaceName);
                    break;
                case "placename_asc":
                    PostalCodes = PostalCodes.OrderByDescending(p => p.PlaceName);
                    break;
                case "placename_desc":
                    PostalCodes = PostalCodes.OrderBy(p => p.PlaceName);
                    break;
                default:
                    PostalCodes = PostalCodes.OrderBy(p => p.Province).ThenBy(p => p.PlaceName);
                    break;
            }
            return PostalCodes;

        }

        public static string NewOrder(string SortOrder, string ColumnName)
        {
            string NewOrder = null;
            switch (ColumnName.ToLower())
            {

                case "postalcode":
                    if (SortOrder == "PostalCode_asc")
                        NewOrder = "PostalCode_desc";
                    else
                        NewOrder = "PostalCode_asc";
                    break;
                case "province":
                    if (SortOrder == "Province_asc")
                        NewOrder = "Province_desc";
                    else
                        NewOrder = "Province_asc";
                    break;
                case "placename":
                    if (SortOrder == "PlaceName_asc")
                        NewOrder = "PlaceName_desc";
                    else
                        NewOrder = "PlaceName_asc";
                    break;
            }
            return NewOrder;
        }

    }
}
