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
    public class BookConditionsController : Controller
    {
        private readonly SeboDbContext _context;

        public BookConditionsController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: BookConditions
        public async Task<IActionResult> Index()
        {
            return View(await _context.BookCondition.ToListAsync());
        }

        // GET: BookConditions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCondition = await _context.BookCondition
                .FirstOrDefaultAsync(m => m.BookConditionId == id);
            if (bookCondition == null)
            {
                return NotFound();
            }

            return View(bookCondition);
        }

        // GET: BookConditions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BookConditions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookConditionId,Condition")] BookCondition bookCondition)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookCondition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookCondition);
        }

        // GET: BookConditions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCondition = await _context.BookCondition.FindAsync(id);
            if (bookCondition == null)
            {
                return NotFound();
            }
            return View(bookCondition);
        }

        // POST: BookConditions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookConditionId,Condition")] BookCondition bookCondition)
        {
            if (id != bookCondition.BookConditionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookCondition);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookConditionExists(bookCondition.BookConditionId))
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
            return View(bookCondition);
        }

        // GET: BookConditions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCondition = await _context.BookCondition
                .FirstOrDefaultAsync(m => m.BookConditionId == id);
            if (bookCondition == null)
            {
                return NotFound();
            }

            return View(bookCondition);
        }

        // POST: BookConditions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookCondition = await _context.BookCondition.FindAsync(id);
            _context.BookCondition.Remove(bookCondition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookConditionExists(int id)
        {
            return _context.BookCondition.Any(e => e.BookConditionId == id);
        }
    }
}
