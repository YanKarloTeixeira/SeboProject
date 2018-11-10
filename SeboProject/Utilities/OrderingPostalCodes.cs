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
            switch (sortOrder)
            {
                case "PostalCode_asc":
                    PostalCodes = PostalCodes.OrderByDescending(p => p.PostalCode);
                    break;
                case "PostalCode_desc":
                    PostalCodes = PostalCodes.OrderBy(p => p.PostalCode);
                    break;
                case "Province_asc":
                    PostalCodes = PostalCodes.OrderByDescending(p => p.Province).ThenBy(p => p.PlaceName);
                    break;
                case "Province_desc":
                    PostalCodes = PostalCodes.OrderBy(p => p.Province).ThenBy(p => p.PlaceName);
                    break;
                case "PlaceName_asc":
                    PostalCodes = PostalCodes.OrderByDescending(p => p.PlaceName);
                    break;
                case "PlaceName_desc":
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
            switch (ColumnName)
            {

                case "PostalCode":
                    if (SortOrder == "PostalCode_asc")
                        NewOrder = "PostalCode_desc";
                    else
                        NewOrder = "PostalCode_asc";
                    break;
                case "Province":
                    if (SortOrder == "Province_asc")
                        NewOrder = "Province_desc";
                    else
                        NewOrder = "Province_asc";
                    break;
                case "PlaceName":
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
