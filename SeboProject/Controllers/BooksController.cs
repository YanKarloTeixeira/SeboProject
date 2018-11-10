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
    public class BooksController : Controller
    {
        private readonly SeboDbContext _context;

        public BooksController(SeboDbContext context)
        {
            _context = context;
        }


        public ActionResult Index2()
        {
            Console.WriteLine("Test");
            return View();
        }
        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, int StudyAreaDropdown, int PublisherDropdown)
        {
            var seboDbContext = _context.Book.Include(b => b.BookCondition).Include(b => b.Seller).Include(b => b.StudyArea);

            
            //var studyArea = (from s in seboDbContext select new { s.StudyAreaId, s.StudyArea.StudyAreaName });
            var studyAreas = (from s in _context.StudyArea orderby s.StudyAreaName  select new { s.StudyAreaId, s.StudyAreaName }).ToList();
            ViewData["studyAreaFilter"] = new SelectList(studyAreas, "StudyAreaId", "StudyAreaName");



            return View(await seboDbContext.ToListAsync());


        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.BookCondition)
                .Include(b => b.Seller)
                .Include(b => b.StudyArea)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewData["BookConditionId"] = new SelectList(_context.BookCondition, "BookConditionId", "Condition");
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator");
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName");
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,Description,ISBN,Publisher,Edition,Quantity,Price,Visualizations,QuantitySold,Blocked,IsWaitList,CreationDate,BookConditionId,StudyAreaId,SellerId")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookConditionId"] = new SelectList(_context.BookCondition, "BookConditionId", "Condition", book.BookConditionId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", book.SellerId);
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName", book.StudyAreaId);
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["BookConditionId"] = new SelectList(_context.BookCondition, "BookConditionId", "Condition", book.BookConditionId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", book.SellerId);
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName", book.StudyAreaId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Description,ISBN,Publisher,Edition,Quantity,Price,Visualizations,QuantitySold,Blocked,IsWaitList,CreationDate,BookConditionId,StudyAreaId,SellerId")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
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
            ViewData["BookConditionId"] = new SelectList(_context.BookCondition, "BookConditionId", "Condition", book.BookConditionId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", book.SellerId);
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName", book.StudyAreaId);
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .Include(b => b.BookCondition)
                .Include(b => b.Seller)
                .Include(b => b.StudyArea)
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
