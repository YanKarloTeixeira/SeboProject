using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
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

        //public async Task<IActionResult> IndexW(string sortOrder, string currentSearchString, string SearchString, int ProvinceFilter, int PlaceNameFilter, int? Page)
        //{
        //    int PageSize = 14; // How many listed items per page definition
        //    var seboDbContext = _context.Book
        //        .Include(b => b.BookCondition)
        //        .Include(b => b.Seller)
        //        .Include(b => b.StudyArea);
        //    var BodyList = (from p in seboDbContext select p); // Item to be listed on the view

        //    if (!String.IsNullOrEmpty(sortOrder))
        //    {
        //        string[] parameter = sortOrder.Split(".");
        //        sortOrder = parameter[0];
        //        ProvinceFilter = Int32.Parse(parameter[1]);
        //        PlaceNameFilter = Int32.Parse(parameter[2]);
        //    }


        //    if (!String.IsNullOrEmpty(SearchString))
        //    {
        //        Page = 1;
        //        BodyList = BodyList.Where(p => p.Title.Contains(SearchString) || p.Description.Contains(SearchString) ||
        //                                                                                      p.Publisher.Contains(SearchString));
        //    }
        //    else currentSearchString = SearchString;

        //    // Applying text filters on the table - It impacts in all dropboxes and lists

        //    //Initializing the queries that belong to the dropboxes
        //    var _PlaceNamesFiltered = BodyList;
        //    var _ProvincesFiltered = BodyList;

        //    // Applying filter over the dropbox query base
        //    if (ProvinceFilter != 0)
        //    {
        //        _PlaceNamesFiltered = from p1 in _PlaceNamesFiltered
        //                              join p2 in _PlaceNamesFiltered on p1.Province equals p2.Province
        //                              where (p1.LocalizationId == ProvinceFilter)
        //                              select p2;
        //    }

        //    /*
        //        Adjusts the BodyList according to the Province selection 
        //        and the PlaceName selection 
        //    */
        //    BodyList = _PlaceNamesFiltered;
        //    if (PlaceNameFilter != 0) BodyList = _PlaceNamesFiltered.Where(p => p.LocalizationId == PlaceNameFilter).Distinct();
        //    if (BodyList.Count() < 1) BodyList = _PlaceNamesFiltered; // In case no match for Province+PlaceName 



        //    //_PlaceNamesFiltered = OrderingPostalCodes.Do(_PlaceNamesFiltered, sortOrder);
        //    /* Ordering before list on the view */
        //    BodyList = OrderingPostalCodes.Do(BodyList, sortOrder);

        //    /*
        //        Preparing DROPBOXLISTs
        //    */
        //    var _Provinces = (from p in _ProvincesFiltered group p by p.Province into pp select pp.First()).ToList();
        //    var _PlaceNames = (from p in _PlaceNamesFiltered orderby p.PlaceName select new { p.LocalizationId, p.PlaceName }).ToList().Distinct();

        //    ViewData["ProvinceFilter"] = new SelectList(_Provinces, "LocalizationId", "Province");
        //    ViewData["PlaceNameFilter"] = new SelectList(_PlaceNames, "LocalizationId", "PlaceName");
        //    /* END of (Preparing DROPBOXLISTs) */

        //    ViewData["SearchString"] = SearchString;
        //    /*
        //     * Adding actual dropboxes selection because in case of a column ordering (by clicking on the column name) 
        //     * the dropbox filtering values would be lost.
        //     */
        //    ViewData["ProvinceOrder"] = OrderingPostalCodes.NewOrder(sortOrder, "Province") + "." + ProvinceFilter + "." + PlaceNameFilter;
        //    ViewData["PlaceNameOrder"] = OrderingPostalCodes.NewOrder(sortOrder, "PlaceName") + "." + ProvinceFilter + "." + PlaceNameFilter;
        //    ViewData["PostalCodeOrder"] = OrderingPostalCodes.NewOrder(sortOrder, "PostalCode") + "." + ProvinceFilter + "." + PlaceNameFilter;

        //    return View(await Pagination<Book>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));

        //}

        public ActionResult Index2()
        {
            Console.WriteLine("Test");
            return View();
        }
        // GET: Books
        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString, int YanFilter, int? Page)
        {
            int PageSize = 14; // How many listed items per page definition
                var seboDbContext = _context.Book.Include(b => b.BookCondition).Include(b => b.Seller).Include(b => b.StudyArea);


            //var studyArea = (from s in seboDbContext select new { s.StudyAreaId, s.StudyArea.StudyAreaName });
            List<string> yan = new List<string>();
            yan.Add("Yes");
            yan.Add("No");
            ViewData["YanFilter"] = new SelectList(yan, "Options");



            //return View(await seboDbContext.ToListAsync());
            var BodyList = (from p in seboDbContext select p); // Item to be listed on the view
            return View(await Pagination<Book>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));


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
