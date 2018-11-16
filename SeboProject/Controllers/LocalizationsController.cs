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
    public class LocalizationsController : Controller
    {
        private readonly SeboDbContext _context;

        public LocalizationsController(SeboDbContext context)
        {
            _context = context;
        }

        
        public async Task<IActionResult> Index(string sortOrder, string currentSearchString, string SearchString, int ProvinceFilter, int PlaceNameFilter, int? Page)
        {
            int PageSize = 14; // How many listed items per page definition
            var seboDbContext = _context.Localization;
            var BodyList = (from p in seboDbContext select p); // Item to be listed on the view

            if (!String.IsNullOrEmpty(sortOrder))
            {
                string[] parameter = sortOrder.Split(".");
                sortOrder = parameter[0];
                ProvinceFilter = Int32.Parse(parameter[1]);
                PlaceNameFilter = Int32.Parse(parameter[2]);
            }


            if (!String.IsNullOrEmpty(SearchString))
            {
                Page = 1;
                BodyList = BodyList.Where(p => p.PostalCode.Contains(SearchString) || p.Province.Contains(SearchString) ||
                                                                                              p.PlaceName.Contains(SearchString));

            }
            else currentSearchString = SearchString;

            // Applying text filters on the table - It impacts in all dropboxes and lists
            
            //Initializing the queries that belong to the dropboxes
            var _PlaceNamesFiltered = BodyList;
            var _ProvincesFiltered = BodyList;

            // Applying filter over the dropbox query base
            if (ProvinceFilter != 0)
            {
                _PlaceNamesFiltered = from p1 in _PlaceNamesFiltered join p2 in _PlaceNamesFiltered on p1.Province equals p2.Province
                                      where (p1.LocalizationId == ProvinceFilter) select p2;
            }

            /*
                Adjusts the BodyList according to the Province selection 
                and the PlaceName selection 
            */ 
            BodyList = _PlaceNamesFiltered;
            if (PlaceNameFilter != 0 ) BodyList = _PlaceNamesFiltered.Where(p => p.LocalizationId == PlaceNameFilter).Distinct();
            if (BodyList.Count() < 1) BodyList = _PlaceNamesFiltered; // In case no match for Province+PlaceName 

            

            //_PlaceNamesFiltered = OrderingPostalCodes.Do(_PlaceNamesFiltered, sortOrder);
            /* Ordering before list on the view */
            BodyList = OrderingPostalCodes.Do(BodyList, sortOrder);

            /*
                Preparing DROPBOXLISTs
            */
            var _Provinces = (from p in _ProvincesFiltered group p by p.Province into pp select pp.First() ).ToList();
            var _PlaceNames = (from p in _PlaceNamesFiltered orderby p.PlaceName select new { p.LocalizationId, p.PlaceName }).ToList().Distinct();

            ViewData["ProvinceFilter"] = new SelectList(_Provinces, "LocalizationId", "Province");
            ViewData["PlaceNameFilter"] = new SelectList(_PlaceNames, "LocalizationId", "PlaceName");
            /* END of (Preparing DROPBOXLISTs) */

            ViewData["SearchString"] = SearchString;
            /*
             * Adding actual dropboxes selection because in case of a column ordering (by clicking on the column name) 
             * the dropbox filtering values would be lost.
             */
            ViewData["ProvinceOrder"] = OrderingPostalCodes.NewOrder(sortOrder, "Province") +"." + ProvinceFilter+ "."+ PlaceNameFilter;
            ViewData["PlaceNameOrder"] = OrderingPostalCodes.NewOrder(sortOrder, "PlaceName")+"." + ProvinceFilter + "." + PlaceNameFilter;
            ViewData["PostalCodeOrder"] = OrderingPostalCodes.NewOrder(sortOrder, "PostalCode")+"." + ProvinceFilter + "." + PlaceNameFilter;

            return View(await Pagination<Localization>.CreateAsync(BodyList.AsNoTracking(), Page ?? 1, PageSize));

        }

        // GET: Localizations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localization = await _context.Localization
                .FirstOrDefaultAsync(m => m.LocalizationId == id);
            if (localization == null)
            {
                return NotFound();
            }

            return View(localization);
        }

        // GET: Localizations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Localizations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalizationId,PostalCode,PlaceName,Province")] Localization localization)
        {
            if (ModelState.IsValid)
            {
                _context.Add(localization);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(localization);
        }

        // GET: Localizations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localization = await _context.Localization.FindAsync(id);
            if (localization == null)
            {
                return NotFound();
            }
            return View(localization);
        }

        // POST: Localizations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocalizationId,PostalCode,PlaceName,Province")] Localization localization)
        {
            if (id != localization.LocalizationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localization);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalizationExists(localization.LocalizationId))
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
            return View(localization);
        }

        // GET: Localizations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localization = await _context.Localization
                .FirstOrDefaultAsync(m => m.LocalizationId == id);
            if (localization == null)
            {
                return NotFound();
            }

            return View(localization);
        }

        // POST: Localizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localization = await _context.Localization.FindAsync(id);
            _context.Localization.Remove(localization);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalizationExists(int id)
        {
            return _context.Localization.Any(e => e.LocalizationId == id);
        }
    }
}
