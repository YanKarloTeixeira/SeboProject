using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
using SeboProject.Helpers;
using SeboProject.Models;
using SeboProject.Utilities;

namespace SeboProject.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SeboDbContext _context;

        public CoursesController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString, int StudyAreaFilter, int InstitutionFilter, int? Page)
        {
            var seboDbContext = _context.Course.Include(c => c.Institution).Include(c => c.StudyArea);
            var courses = (from s in seboDbContext select s);

            if (currentSearchString != null)
            {
                string[] m = currentSearchString.Split(".");

                currentSearchString = m[0]; // preserves the current filter typed within the search textbox
               

                if (SearchString == null)
                {
                    SearchString = currentSearchString;
                }
                if (StudyAreaFilter == 0)
                {
                    InstitutionFilter = Int32.Parse(m[1]);   // preserves the select institution option in the dropdownlist
                }
                if (InstitutionFilter == 0)
                {
                    StudyAreaFilter = Int32.Parse(m[2]);     // preserves the select Study Area option in the dropdownlist
                }
            }


            // Applying filters on the table
            //if (!String.IsNullOrEmpty(SearchString)) courses = courses.Where(c => c.CourseName.Contains(SearchString));
            if (!String.IsNullOrEmpty(SearchString)) {
                var myString =  SearchString.Trim().Split(" ");
                //courses = courses.Where(c => myString.All(m => c.CourseName.ToLower().Contains(m.ToLower())));
                courses = StringSearch.SearchCourseName(_context, myString).Include(c => c.Institution).Include(c => c.StudyArea);
             }
            if (InstitutionFilter != 0) courses = courses.Where(c => c.InstitutionId == InstitutionFilter);
            if (StudyAreaFilter != 0) courses = courses.Where(c => c.StudyAreaId == StudyAreaFilter);

            
            courses = OrderingCourses.Do(courses, sortOrder);
            //////////////////////////////////////
            //Preparing Dropboxes 
            //////////////////////////////////////
            var StudyAreas = (from s in courses orderby s.StudyArea.StudyAreaName select new { s.StudyAreaId, s.StudyArea.StudyAreaName }).ToList().Distinct();
            var Institutions = (from i in courses orderby i.Institution.InstitutionName select new { i.InstitutionId, i.Institution.InstitutionName }).ToList().Distinct();

            ViewData["StudyAreaFilter"] = new SelectList(StudyAreas, "StudyAreaId", "StudyAreaName");
            ViewData["InstitutionFilter"] = new SelectList(Institutions, "InstitutionId", "InstitutionName");
            //////////////////////////////////////

            ViewData["CurrentSearchString"] = SearchString + "." + InstitutionFilter + "." + StudyAreaFilter + "." + Page;
            ViewData["SearchString"] = SearchString;
            ViewData["Institution"] = OrderingCourses.NewOrder(sortOrder, "Institution");
            ViewData["StudyArea"] = OrderingCourses.NewOrder(sortOrder, "StudyArea");
            ViewData["CourseName"] = OrderingCourses.NewOrder(sortOrder, "CourseName");

            //return View(await courses.ToListAsync());
            int PageSize = 14;
            return View(await Pagination<Course>.CreateAsync(courses.AsNoTracking(), Page ?? 1, PageSize));

        }




        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Institution)
                .Include(c => c.StudyArea)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["InstitutionId"] = new SelectList(_context.Institution.OrderBy(i => i.InstitutionName), "InstitutionId", "InstitutionName");
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea.OrderBy(s => s.StudyAreaName), "StudyAreaId", "StudyAreaName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,StudyAreaId,InstitutionId")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstitutionId"] = new SelectList(_context.Institution.OrderBy(i => i.InstitutionName), "InstitutionId", "InstitutionName");
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea.OrderBy(s => s.StudyAreaName), "StudyAreaId", "StudyAreaName");
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["InstitutionId"] = new SelectList(_context.Institution.OrderBy(i => i.InstitutionName), "InstitutionId", "InstitutionName");
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea.OrderBy(s => s.StudyAreaName), "StudyAreaId", "StudyAreaName");
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,StudyAreaId,InstitutionId")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstitutionId"] = new SelectList(_context.Institution.OrderBy(i => i.InstitutionName), "InstitutionId", "InstitutionName");
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea.OrderBy(s => s.StudyAreaName), "StudyAreaId", "StudyAreaName");
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.Institution)
                .Include(c => c.StudyArea)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course.FindAsync(id);
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.CourseId == id);
        }
    }
}
