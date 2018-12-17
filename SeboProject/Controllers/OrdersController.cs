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
            int userId = HelperUser.GetUserId(this.User.Identity.Name, _context);
            
            var seboDbContext = _context.Order.Include(o => o.Book).Include(o => o.User)
                .Where(o=> 
                    HelperUser.isAdministrator(this.User.Identity.Name)? o.UserId>0 : o.UserId==userId || 
                    HelperUser.isAdministrator(this.User.Identity.Name) ? o.SellerId > 0 : o.SellerId == userId
                );

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
                .Include(o => o.User)
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
            if (HelperUser.isAdministrator(this.User.Identity.Name))
            {
                ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN");
                ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName");
                return View();
            }

            return RedirectToAction("Index");
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,Quantity,Price,PaymentForm,CreationDate,PaymentDate,ReleaseDate,CancelationDate,CanfirmationDate,Status,IsConfirmedByBuyer,BookId,UserId,SellerId,Seller")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN", order.BookId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", order.UserId);
            return View(order);
        }


        // GET: Orders/Create
        public async Task<IActionResult> BuyIt(int id=0)
        {
            Order order = new Order();
            if (id == 0)
            {
                return NotFound();
            }

            var book = _context.Book.Include(b => b.BookCondition).Include(b => b.StudyArea).Where(b=>b.BookId==id).First();
            
            if (book == null)
            {
                return NotFound();
            }

            order.BookId = id;
            order.CreationDate = DateTime.Now;
            order.PaymentForm = "c";
            order.Price = book.Price;
            order.Quantity = 1;
            order.SellerId = book.UserId;
            order.UserId = HelperUser.GetUserId(this.User.Identity.Name, _context);

            ViewData["BookTitle"] = book.Title;
            ViewData["StudyArea"] = book.StudyArea.StudyAreaName;
            ViewData["BookCondition"] = book.BookCondition.Condition;
            ViewData["MaxQty"] = book.Quantity;

            //ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN", order.BookId);
            //ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", order.UserId);
            return View(order);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> BuyIt([Bind("OrderId,Quantity,Price,PaymentForm,CreationDate,PaymentDate,ReleaseDate,CancelationDate,CanfirmationDate,Status,IsConfirmedByBuyer,BookId,UserId,SellerId,Seller")] Order order)
        public async Task<IActionResult> BuyIt([Bind("OrderId,Quantity,Price,PaymentForm,CreationDate,PaymentDate,ReleaseDate,CancelationDate,CanfirmationDate,Status,IsConfirmedByBuyer,BookId,UserId,SellerId,Seller")] Order order)
        {
            if (ModelState.IsValid)
            {
                var order2= _context.Add(order);
                await _context.SaveChangesAsync();
                Book book = (from b in _context.Book
                            where b.BookId == order.BookId
                            select b).First();
                book.Quantity -= order.Quantity;
                book.QuantitySold += order.Quantity;

                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("SuccessfulTransactionMsg", order);
            }
            ViewData["BookId"] = new SelectList(_context.Book, "BookId", "ISBN", order.BookId);
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", order.UserId);
            return View(order);
        }
        public async Task<IActionResult> SuccessfulTransactionMsg(Order order)
        {
            //var order = await _context.Order.FindAsync(OrderId);
            var seller = (from s in _context.User where s.UserId == order.SellerId select s).First();
            var buyer = (from s in _context.User where s.UserId == order.UserId select s).First();
            var book  =  (from bo in _context.Book where bo.BookId == order.BookId select bo).Include(b => b.BookCondition).Include(b => b.StudyArea).First();

            @ViewData["SellerName"] = seller.FirstName + " " + seller.MiddleName + " " + seller.LastName;
            @ViewData["SellerEmail"] = seller.Email;
            @ViewData["SellerPhone"] = seller.Phone;


            //Buyer Email


            string msg = null;
            if (!SeboEmail.SendReceiptToBuyer(order, buyer, seller, book)) {
                msg = "ERROR : The attempt to send the receipt of the transaction for your email has failed.\nPlease check your registered email.";
            }
            if (!SeboEmail.SendReceiptToSeller(order, buyer, seller, book)) {
                msg = "ERROR : The attempt to send the receipt of the transaction for seller's email has failed.\nPlease, contact the seller as soons as possible.";
            }
            if (String.IsNullOrEmpty(msg))
            {
                msg = "The receipt of this transaction was sent for your email.";
            }

            ViewData["ClosingMsg"] = msg;
            return View();
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (!HelperUser.isAdministrator(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }

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
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Quantity,Price,PaymentForm,CreationDate,PaymentDate,ReleaseDate,CancelationDate,CanfirmationDate,Status,IsConfirmedByBuyer,BookId,UserId,SellerId,Seller")] Order order)
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
            ViewData["UserId"] = new SelectList(_context.User, "UserId", "CreditcardName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (!HelperUser.isAdministrator(this.User.Identity.Name))
            {
                return RedirectToAction("Index");
            }

            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Book)
                .Include(o => o.User)
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
