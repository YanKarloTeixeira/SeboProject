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
            if (String.IsNullOrEmpty(sortOrder)) sortOrder = "institution_desc";
            switch (sortOrder.ToLower())
            {
                case "institution_asc":
                    courses = courses.OrderByDescending(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "institution_desc":
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "studyarea_asc":
                    courses = courses.OrderByDescending(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "studyarea_desc":
                    courses = courses.OrderBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "course_asc":
                    courses = courses.OrderByDescending(c => c.CourseName);
                    break;
                case "course_desc":
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
            switch (ColumnName.ToLower()) { 
            
                case "institution":
                    if(SortOrder== "Institution_asc")
                        NewOrder ="Institution_desc";
                    else 
                        NewOrder = "Institution_asc";
                    break;
                case "studyarea":
                    if (SortOrder == "StudyArea_asc")
                        NewOrder = "StudyArea_desc";
                    else
                        NewOrder = "StudyArea_asc";
                    break;
                case "coursename":
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
