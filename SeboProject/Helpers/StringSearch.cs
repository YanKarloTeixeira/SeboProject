using SeboProject.Data;
using SeboProject.Models;
using SeboProject.Utilities;
using System.Linq;

namespace SeboProject.Helpers
{
    public class StringSearch
    {
        public static IQueryable<Course> CourseName(SeboDbContext _context, params string[] keywords )
        {
            
            IQueryable<Course> query = from q in _context.Course select q;
            foreach (string keyword in keywords)
            {
                string temp = keyword;
                query = query.Where(p => p.CourseName.Contains(temp));
            }
            return query;
        }
        public static IQueryable<Course> SearchCourseName(SeboDbContext _context, params string[] keywords)
        {
            var predicate = PredicateBuilder.False<Course>();

            foreach (string keyword in keywords)
            {
                string temp = keyword;
                predicate = predicate.Or(p => p.CourseName.Contains(temp));
            }
            return  (from s in _context.Course select s).Where(predicate);
        }
        public static IQueryable<Book> SearchBook(SeboDbContext _context, string sortOrder, params string[] keywords)
        {
            IQueryable<Book> books = (from b in _context.Book select b);

            if (keywords.Length > 0)
            {
                var predicate = PredicateBuilder.False<Book>();
                foreach (string keyword in keywords)
                {
                    string temp = keyword;
                    predicate = predicate.Or(p => p.Title.Contains(temp)).Or(p => p.Description.Contains(temp)).Or(p => p.ISBN.Contains(temp));
                }
                books = (from b in _context.Book select b).Where(predicate);
            }

            books = OrderingBooks.Do(books, sortOrder);
            return books;
        }
    }
}
