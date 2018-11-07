using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
using SeboProject.Models;

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
        public async Task<IActionResult> Index(string sortOrder)
        {
             var seboDbContext = _context.Course.Include(c => c.Institution).Include(c => c.StudyArea);
            //var courses = (from s in seboDbContext orderby s.Institution.InstitutionName, s.StudyArea.StudyAreaName, s.CourseName select s).ToListAsync();
            var courses = (from s in seboDbContext select s);

            switch (sortOrder)
            {
                case "Institution_asc":
                    ViewData["Institution"] = "Institution_desc";
                    //ViewData["StudyArea"] = "StudyArea_asc";
                    //ViewData["CourseName"] = "Course_asc";
                    courses = courses.OrderByDescending(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "Institution_desc":
                    ViewData["Institution"] = "Institution_asc";
                    //ViewData["StudyArea"] = "StudyArea_asc";
                    //ViewData["CourseName"] = "Course_asc";
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "StudyArea_asc":
                    ViewData["StudyArea"] = "StudyArea_desc";
                    //ViewData["Institution"] = "";
                    //ViewData["CourseName"] = "Course_asc";
                    courses = courses.OrderByDescending(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "StudyArea_desc":
                    ViewData["StudyArea"] = "StudyArea_asc";
                    //ViewData["Institution"] = "";
                    //ViewData["CourseName"] = "Course_asc";
                    courses = courses.OrderBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;
                case "Course_asc":
                    ViewData["CourseName"] = "Course_desc";
                    courses = courses.OrderByDescending(c => c.CourseName);
                    break;
                case "Course_desc":
                    ViewData["CourseName"] = "Course_asc";
                    courses = courses.OrderBy(c => c.CourseName);
                    break;
                default:
                    ViewData["Institution"] = "Institution_asc";
                    ViewData["StudyArea"] = "StudyArea_asc";
                    ViewData["CourseName"] = "Course_asc";
                    courses = courses.OrderBy(c => c.Institution.InstitutionName).ThenBy(c => c.StudyArea.StudyAreaName).ThenBy(c => c.CourseName);
                    break;

            }
            return View(await courses.ToListAsync());
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
            ViewData["InstitutionId"] = new SelectList(_context.Institution.OrderBy(i=>i.InstitutionName), "InstitutionId", "InstitutionName");
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
