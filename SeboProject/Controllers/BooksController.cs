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
using SeboProject.Utilities;

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
        public async Task<IActionResult> BooksCatalog(string UserName, string sortOrder, string currentSearchString, string SearchString, int StudyAreaFilter, int BookConditionFilter, int? Page)
        {

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
                    StudyAreaFilter = Int32.Parse(m[1]);   // preserves the select institution option in the dropdownlist
                }
                if (BookConditionFilter == 0)
                {
                    BookConditionFilter = Int32.Parse(m[2]);     // preserves the select Study Area option in the dropdownlist
                }
            }
            //Get the User ID 
            int UserId = HelperUser.GetUserId(this.User.Identity.Name, _context);
            int UserBranchId = HelperUser.GetUserBranchId(this.User.Identity.Name, _context);

            // Cheks whether a search string was typed and prepares for search by each word
            string[]  myString = SearchString!=null ? SearchString.Trim().Split(" ") : new string[0];

            // Get the seach's recordset already sorted
            var books = StringSearch.SearchBook(_context, sortOrder, myString).Include(b => b.BookCondition).Include(b => b.StudyArea).Include(b => b.User)
                .Where(b => !b.Blocked)
                .Where(b => b.Quantity > b.QuantitySold)
                .Where(b => b.UserId!= UserId)
                .Where(b => !b.IsWaitList);

            var users = (from u in _context.User select u).Include(u => u.InstitutionBranch);
            //Getting the books from the same institution
            var books2 = (from b in books
                         join u in users on b.UserId equals u.UserId
                         select new
                         {
                             b.Blocked,
                             b.BookCondition,
                             b.BookConditionId,
                             b.BookId,
                             b.CreationDate,
                             b.Description,
                             b.Edition,
                             b.ISBN,
                             b.IsWaitList,
                             b.Orders,
                             b.PhotoFileName,
                             b.Price,
                             b.Publisher,
                             b.Quantity,
                             b.QuantitySold,
                             b.StudyArea,
                             b.StudyAreaId,
                             b.Title,
                             b.User,
                             b.UserId,
                             b.Visualizations,
                             u.InstitutionBranchId,
                             u.InstitutionBranch.InstitutionBranchName
                         }).Where(b=>b.InstitutionBranchId==UserBranchId);

            // Applying filters on the table
            if (StudyAreaFilter != 0) books2 = books2.Where(b => b.StudyAreaId == StudyAreaFilter);
            if (BookConditionFilter != 0) books2 = books2.Where(b => b.BookConditionId == BookConditionFilter);

            //
            // Preparing Dropboxes 
            //
            var StudyAreas = (from s in books2 orderby s.StudyArea.StudyAreaName select new { s.StudyAreaId, s.StudyArea.StudyAreaName }).ToList().Distinct();
            var BookConditions = (from b in books2 orderby b.BookCondition select new { b.BookConditionId, b.BookCondition.Condition }).ToList().Distinct();

            ViewData["StudyAreaFilter"] = new SelectList(StudyAreas, "StudyAreaId", "StudyAreaName");
            ViewData["BookConditionFilter"] = new SelectList(BookConditions, "BookConditionId", "Condition");
            //////////////////////////////////////

            ViewData["CurrentSearchString"] = SearchString +"." + StudyAreaFilter + "." + BookConditionFilter + "." + Page;
            ViewData["SearchString"] = SearchString;

            //
            // Just tracking which ordering column was trigged for setting the actual ordering
            //
            ViewData["Title"] = OrderingBooks.NewOrder(sortOrder, "Title");
            ViewData["StudyArea"] = OrderingBooks.NewOrder(sortOrder, "StudyArea");
            ViewData["BookCondition"] = OrderingBooks.NewOrder(sortOrder, "BookCondition");
            ViewData["ISBN"] = OrderingBooks.NewOrder(sortOrder, "ISBN");
            ViewData["Price"] = OrderingBooks.NewOrder(sortOrder, "Price");

            int PageSize = 14;
            return View(await Pagination<Book>.CreateAsync(books.AsNoTracking(), Page ?? 1, PageSize));
        }

        public async Task<IActionResult> Index(string UserName, string sortOrder, string currentSearchString, string SearchString, int StudyAreaFilter, int BookConditionFilter, int? Page)
        {
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
                    StudyAreaFilter = Int32.Parse(m[1]);   // preserves the select institution option in the dropdownlist
                }
                if (BookConditionFilter == 0)
                {
                    BookConditionFilter = Int32.Parse(m[2]);     // preserves the select Study Area option in the dropdownlist
                }
            }
            //Get the User ID
            if(String.IsNullOrEmpty(UserName)) UserName = this.User.Identity.Name;
            int UserId = HelperUser.GetUserId(UserName, _context);

            // Cheks whether a search string was typed and prepares for search by each word
            string[] myString = SearchString != null ? SearchString.Trim().Split(" ") : new string[0];
            var x = HelperUser.isAdministrator(UserName);
            // Get the seach's recordset already sorted applying filtering according to user role
            var books = StringSearch.SearchBook(_context, sortOrder, myString).Include(b => b.BookCondition).Include(b => b.StudyArea).Include(b => b.User)
                .Where(b => !b.Blocked)
                .Where(b => b.Quantity > b.QuantitySold)
                .Where(b => !b.IsWaitList)
                .Where(b=> HelperUser.isAdministrator(UserName) ? b.UserId > 0 : b.UserId == UserId);
                //.Where(b => UserId > 0 || UserName != null ? b.UserId == UserId : b.UserId > 0)

            // Applying filters on the table
            if (StudyAreaFilter != 0) books = books.Where(b => b.StudyAreaId == StudyAreaFilter);
            if (BookConditionFilter != 0) books = books.Where(b => b.BookConditionId == BookConditionFilter);

            //
            // Preparing Dropboxes 
            //
            var StudyAreas = (from s in books orderby s.StudyArea.StudyAreaName select new { s.StudyAreaId, s.StudyArea.StudyAreaName }).ToList().Distinct();
            var BookConditions = (from b in books orderby b.BookCondition select new { b.BookConditionId, b.BookCondition.Condition }).ToList().Distinct();

            ViewData["StudyAreaFilter"] = new SelectList(StudyAreas, "StudyAreaId", "StudyAreaName");
            ViewData["BookConditionFilter"] = new SelectList(BookConditions, "BookConditionId", "Condition");
            //////////////////////////////////////

            ViewData["CurrentSearchString"] = SearchString + "." + StudyAreaFilter + "." + BookConditionFilter + "." + Page;
            ViewData["SearchString"] = SearchString;

            //
            // Just tracking which ordering column was trigged for setting the actual ordering
            //
            ViewData["Title"] = OrderingBooks.NewOrder(sortOrder, "Title");
            ViewData["StudyArea"] = OrderingBooks.NewOrder(sortOrder, "StudyArea");
            ViewData["BookCondition"] = OrderingBooks.NewOrder(sortOrder, "BookCondition");
            ViewData["ISBN"] = OrderingBooks.NewOrder(sortOrder, "ISBN");
            ViewData["Price"] = OrderingBooks.NewOrder(sortOrder, "Price");

            int PageSize = 14;
            return View(await Pagination<Book>.CreateAsync(books.AsNoTracking(), Page ?? 1, PageSize));
        }



        public async Task<IActionResult> Welcome()
        {
            int UserId = HelperUser.GetUserId(this.User.Identity.Name, _context);


            var seboDbContext = _context.Book.Include(b => b.BookCondition)
                .Include(b => b.StudyArea)
                .Include(b => b.User)

                .Where(b => b.Quantity > b.QuantitySold)
                .Where(b => HelperUser.isAdministrator(this.User.Identity.Name) || String.IsNullOrEmpty(this.User.Identity.Name) ? b.UserId > 0 : b.UserId == UserId);
            //.Where(b => b.UserId != UserId)


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


        public async Task<IActionResult> BookCatalogDetails(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var bk = (from b in _context.Book where b.BookId == book.BookId select b.PhotoFileName).ToArray();
            Book bookVisualized = (from b in _context.Book
                                   where b.BookId == id
                                   select b).SingleOrDefault();

            bookVisualized.Visualizations++;
            _context.Book.Update(bookVisualized);
            await _context.SaveChangesAsync();

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
            long size = 0;
            
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
                    //using (MemoryStream ms = new MemoryStream())
                    //{
                    //    await PhotoFileName.CopyToAsync(ms);
                    //    book.PhotoFileName = ms.ToArray();
                    //}
                    MemoryStream ms = new MemoryStream();
                    await PhotoFileName.CopyToAsync(ms);
                    book.PhotoFileName = ms.ToArray();
                    size= (PhotoFileName.Length / 1024 ) ;
                }



                if (id != book.BookId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid && size<=2048)
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
