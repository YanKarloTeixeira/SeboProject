using SeboProject.Data;
using SeboProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
