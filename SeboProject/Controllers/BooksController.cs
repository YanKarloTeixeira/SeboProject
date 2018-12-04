using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
using SeboProject.Helpers;
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

        // GET: Books
        public async Task<IActionResult> Index(string UserName)
        {
            int UserId = HelperUser.GetUserId(UserName, _context);
            if(UserId > 0)
            {
                var seboDbContext = _context.Book.Include(b => b.BookCondition).Include(b => b.StudyArea).Include(b => b.User).Where(b => b.UserId == UserId);
                return View(await seboDbContext.ToListAsync());
            }
            else {
                var seboDbContext = _context.Book.Include(b => b.BookCondition).Include(b => b.StudyArea).Include(b => b.User);
                return View(await seboDbContext.ToListAsync());
            }

        }
        public async Task<IActionResult> Welcome()
        {
            var seboDbContext = _context.Book.Include(b => b.BookCondition).Include(b => b.StudyArea).Include(b => b.User);
            return View(await seboDbContext.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int BookId, string Title, string Description, string ISBN, string Publisher
                                          , int Edition, int Quantity, double Price, int BookConditionId, int StudyAreaId,
                                          IFormFile PhotoFileName)
        {
            Book book = new Book
            {
                Blocked = false,
                BookConditionId = BookConditionId,
                CreationDate = DateTime.Now,
                Description = Description,
                Edition = Edition,
                ISBN = ISBN,
                IsWaitList = false,
                Price = Price,
                Publisher = Publisher,
                Quantity = Quantity,
                QuantitySold = 0,
                StudyAreaId = StudyAreaId,
                Title = Title
            };

            string LogedUser = this.User.Identity.Name;
            var user = (from s in _context.User where s.UserName == LogedUser select s.UserId).ToList();
            int UserId = user[0];

            if (UserId > 0)
            {
                book.UserId = UserId;
                if (PhotoFileName != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await PhotoFileName.CopyToAsync(ms);
                        book.PhotoFileName = ms.ToArray();
                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(book);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["BookConditionId"] = new SelectList(_context.BookCondition, "BookConditionId", "Condition", book.BookConditionId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", book.UserId);
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName", book.StudyAreaId);
            return View(book);
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id==0)
            {
                return NotFound();
            }
            //var bk = (from b in _context.Book where b.BookId == book.BookId select b.PhotoFileName).ToArray();
            Book bookVisualized = (from b in _context.Book where b.BookId == id
                            select b).SingleOrDefault();

            bookVisualized.Visualizations++;

            _context.SaveChanges();
            var book = await _context.Book
                .Include(b => b.BookCondition)
                .Include(b => b.StudyArea)
                .Include(b => b.User)
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
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName");
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName");
            return View();
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
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName", book.StudyAreaId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", book.UserId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int BookId, string Title, string Description, string ISBN, string Publisher
                                          , int Edition, int Quantity, double Price, int BookConditionId, int StudyAreaId,
                                          IFormFile PhotoFileName)
        //public async Task<IActionResult> Edit(int id, BookId,PhotoFileName,Title,Description,ISBN,Publisher,Edition,Quantity,Price,Visualizations,QuantitySold,Blocked,IsWaitList,CreationDate,BookConditionId,StudyAreaId,UserId)
        //
        {
            Book book = new Book
            {
                BookId = BookId,
                Blocked = false,
                BookConditionId = BookConditionId,
                CreationDate = DateTime.Now,
                Description = Description,
                Edition = Edition,
                ISBN = ISBN,
                IsWaitList = false,
                Price = Price,
                Publisher = Publisher,
                Quantity = Quantity,
                QuantitySold = 0,
                StudyAreaId = StudyAreaId,
                Title = Title
            };

            string LogedUser = this.User.Identity.Name;
            var user = (from s in _context.User where s.UserName == LogedUser select s.UserId).ToList();
            int UserId = user[0];

            if (UserId > 0)
            {
                book.UserId = UserId;
                if (PhotoFileName != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await PhotoFileName.CopyToAsync(ms);
                        book.PhotoFileName = ms.ToArray();
                    }
                }
                


                if (id != book.BookId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        if (BookExists(book.BookId) && PhotoFileName==null)
                        {
                            var bk = (from b in _context.Book where b.BookId == book.BookId select b.PhotoFileName).ToArray();
                            book.PhotoFileName = bk[0];
                        }

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
            }
            ViewData["BookConditionId"] = new SelectList(_context.BookCondition, "BookConditionId", "Condition", book.BookConditionId);
            ViewData["StudyAreaId"] = new SelectList(_context.StudyArea, "StudyAreaId", "StudyAreaName", book.StudyAreaId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", book.UserId);
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
                .Include(b => b.StudyArea)
                .Include(b => b.User)
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
