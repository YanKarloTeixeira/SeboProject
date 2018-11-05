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
    public class OrdersController : Controller
    {
        private readonly SeboDbContext _context;

        public OrdersController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var seboDbContext = _context.Order.Include(o => o.Book).Include(o => o.Buyer).Include(o => o.Seller);
            return View(await seboDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Book)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN");
            ViewData["BuyerId"] = new SelectList(_context.Set<Buyer>(), "UserId", "Discriminator");
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Quantity,Price,PaymentForm,CreationDate,PaymentDate,ReleaseDate,CancelationDate,CanfirmationDate,Status,IsConfirmedByBuyer,BookId,BuyerId,SellerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN", order.BookId);
            ViewData["BuyerId"] = new SelectList(_context.Set<Buyer>(), "UserId", "Discriminator", order.BuyerId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", order.SellerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN", order.BookId);
            ViewData["BuyerId"] = new SelectList(_context.Set<Buyer>(), "UserId", "Discriminator", order.BuyerId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", order.SellerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Quantity,Price,PaymentForm,CreationDate,PaymentDate,ReleaseDate,CancelationDate,CanfirmationDate,Status,IsConfirmedByBuyer,BookId,BuyerId,SellerId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN", order.BookId);
            ViewData["BuyerId"] = new SelectList(_context.Set<Buyer>(), "UserId", "Discriminator", order.BuyerId);
            ViewData["SellerId"] = new SelectList(_context.Set<Seller>(), "UserId", "Discriminator", order.SellerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Book)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderId == id);
        }
    }
}
