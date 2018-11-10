using SeboProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeboProject.Utilities
{
    public class OrderingCourses
    {

        public static IQueryable<Course> Do(IQueryable<Course> courses, string sortOrder)
        {
            switch (sortOrder)
            {
                case "Institution_asc":
                    courses = courses.OrderByDescending(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "Institution_desc":
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "StudyArea_asc":
                    courses = courses.OrderByDescending(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "StudyArea_desc":
                    courses = courses.OrderBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "Course_asc":
                    courses = courses.OrderByDescending(c => c.CourseName);
                    break;
                case "Course_desc":
                    courses = courses.OrderBy(c => c.CourseName);
                    break;
                default:
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
            }
            return courses;

        }

        public static string NewOrder(string SortOrder, string ColumnName)
        {
            string NewOrder = null;
            switch(ColumnName) { 
            
                case "Institution":
                    if(SortOrder== "Institution_asc")
                        NewOrder ="Institution_desc";
                    else 
                        NewOrder = "Institution_asc";
                    break;
                case "StudyArea":
                    if (SortOrder == "StudyArea_asc")
                        NewOrder = "StudyArea_desc";
                    else
                        NewOrder = "StudyArea_asc";
                    break;
                case "CourseName":
                    if (SortOrder == "Course_asc")
                        NewOrder = "Course_desc";
                    else
                        NewOrder = "Course_asc";
                    break;
            }
            return NewOrder;
        }



    }
}
