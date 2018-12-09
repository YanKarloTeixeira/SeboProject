using SeboProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeboProject.Utilities
{
    public class OrderingBooks
    {

        public static IQueryable<Book> Do(IQueryable<Book> books, string sortOrder)
        {
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "title_desc";
            switch (sortOrder.ToLower())
            {
                case "title_asc":
                    books = books.OrderByDescending(b => b.Title).ThenBy(b => b.Price).ThenBy(b => b.CreationDate);
                    break;
                case "title_desc":
                    books = books.OrderBy(b => b.Title).ThenBy(b => b.Price).ThenBy(b => b.CreationDate);
                    break;
                case "studyarea_asc":
                    books = books.OrderByDescending(b => b.StudyArea.StudyAreaName).ThenBy(b => b.Title).ThenBy(b => b.Price);
                    break;
                case "studyarea_desc":
                    books = books.OrderBy(b => b.StudyArea.StudyAreaName).ThenBy(b => b.Title).ThenBy(b => b.Price);
                    break;
                case "bookcondition_asc":
                    books = books.OrderByDescending(b => b.BookCondition).ThenBy(b => b.Title).ThenBy(b => b.Price);
                    break;
                case "bookcondition_desc":
                    books = books.OrderBy(b => b.BookCondition).ThenBy(b => b.Title).ThenBy(b => b.Price);
                    break;
                case "isbn_asc":
                    books = books.OrderByDescending(b => b.ISBN).ThenBy(b => b.Price).ThenBy(b => b.CreationDate); 
                    break;
                case "isbn_desc":
                    books = books.OrderBy(b => b.ISBN).ThenBy(b => b.Price).ThenBy(b => b.CreationDate);
                    break;
                case "price_asc":
                    books = books.OrderByDescending(b => b.ISBN).ThenBy(b => b.Price).ThenBy(b => b.CreationDate);
                    break;
                case "price_desc":
                    books = books.OrderBy(b => b.Price).ThenBy(b => b.Title).ThenBy(b => b.CreationDate);
                    break;
                default:
                    books = books.OrderBy(b => b.Price).ThenBy(b => b.Title).ThenBy(b => b.CreationDate);
                    break;
            }
            return books;

        }

        public static string NewOrder(string SortOrder, string ColumnName)
        {
            string NewOrder = null;
            switch (ColumnName.ToLower())
            {

                case "title":
                    if (SortOrder == "title_asc")
                        NewOrder = "title_desc";
                    else
                        NewOrder = "title_asc";
                    break;
                case "studyarea":
                    if (SortOrder == "StudyArea_asc")
                        NewOrder = "StudyArea_desc";
                    else
                        NewOrder = "StudyArea_asc";
                    break;
                case "bookcondition":
                    if (SortOrder == "bookcondition_asc")
                        NewOrder = "bookcondition_desc";
                    else
                        NewOrder = "bookcondition_asc";
                    break;
                case "isbn":
                    if (SortOrder == "isbn_asc")
                        NewOrder = "isbn_desc";
                    else
                        NewOrder = "isbn_asc";
                    break;
                case "price":
                    if (SortOrder == "price_asc")
                        NewOrder = "price_desc";
                    else
                        NewOrder = "price_asc";
                    break;
            }
            return NewOrder;
        }



    }
}
