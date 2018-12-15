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
    public class UsersController : Controller
    {
        private readonly SeboDbContext _context;

        public UsersController(SeboDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var seboDbContext = _context.User.Include(u => u.InstitutionBranch).Include(u => u.Localization);
            return View(await seboDbContext.ToListAsync());
        }

        public Task<IActionResult> UserProfileSelfAdm()
        {
            string LogedUser = this.User.Identity.Name;
            var user = (from s in _context.User where s.UserName == LogedUser select s.UserId).ToList();
            int UserId = user[0];
            Task<IActionResult> action = Details(UserId);

            
            return action;
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["PreviousPage"] = "";
            if (id == null)
            {
                string LogedUser = this.User.Identity.Name;
                var u = (from s in _context.User where s.UserName == LogedUser select s.UserId).ToList();
                if (u.Count() < 1) { return NotFound(); }
                int UserId = u[0];
                if (UserId > 0)
                {
                    id = UserId;
                    ViewData["PreviousPage"] = "Home";
                }
                else
                {
                    return NotFound();
                }

            }

            var user = await _context.User
                .Include(u => u.InstitutionBranch)
                .Include(u => u.Localization)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create

        [HttpPost]
        public IActionResult Test(string InstitutionFilter = null)
        {


            ViewData["InstitutionFilter"] = new SelectList(_context.Institution, "InstitutionId", "InstitutionName");
            if (String.IsNullOrEmpty(InstitutionFilter))
                ViewData["InstitutionBranchId"] = new SelectList(_context.InstitutionBranch, "InstitutionBranchId", "InstitutionBranchName");
            else
                ViewData["InstitutionBranchId"] = new SelectList(_context.InstitutionBranch.Where(b => b.InstitutionId.ToString() == InstitutionFilter), "InstitutionBranchId", "InstitutionBranchName");
            ViewData["LocalizationId"] = new SelectList(_context.Localization, "LocalizationId", "PlaceName");
            return View();
        }
        public IActionResult Create(string InstitutionFilter = null)
        {

            var lista = from l in _context.InstitutionBranch.Include(b=> b.Institution) select new { l.InstitutionBranchId, item ="Institution : "+l.Institution.InstitutionName +" / Campus : "+l.InstitutionBranchName, };
            ViewData["InstitutionBranchId"] = new SelectList(lista, "InstitutionBranchId", "item");
            ViewData["LocalizationId"] = new SelectList(_context.Localization, "LocalizationId", "PlaceName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("UserId,UserName,FirstName,MiddleName,LastName,UserType,Address,Number,AddressComplement,Age,Email,Phone,Creditcard,CreditcardName,LocalizationId,InstitutionBranchId")] User user)
        public async Task<IActionResult> Create([Bind("UserId,UserName,FirstName,MiddleName,LastName,UserType,Address,Number,AddressComplement,Age,Email,Phone,Creditcard,CreditcardName,LocalizationId,InstitutionBranchId")] User user)
        {
            user.Email = user.UserName;
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("BooksCatalog", "Books");
            }
            //ViewData["InstitutionBranchId"] = new SelectList(_context.InstitutionBranch, "InstitutionBranchId", "InstitutionBranchName", user.InstitutionBranchId);
            //ViewData["LocalizationId"] = new SelectList(_context.Localization, "LocalizationId", "PlaceName", user.LocalizationId);
            var lista = from l in _context.InstitutionBranch.Include(b => b.Institution) select new { l.InstitutionBranchId, item = "Institution : " + l.Institution.InstitutionName + " / Campus : " + l.InstitutionBranchName, };
            ViewData["InstitutionBranchId"] = new SelectList(lista, "InstitutionBranchId", "item");
            ViewData["LocalizationId"] = new SelectList(_context.Localization, "LocalizationId", "PlaceName");
            return View();

        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                string LogedUser = this.User.Identity.Name;
                var u = (from s in _context.User where s.UserName == LogedUser select s.UserId).ToList();
                int UserId = u[0];
                if (UserId > 0)
                {
                    id = UserId;
                }
                else
                {
                    return NotFound();
                }
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["InstitutionBranchId"] = new SelectList(_context.InstitutionBranch, "InstitutionBranchId", "InstitutionBranchName", user.InstitutionBranchId);
            ViewData["LocalizationId"] = new SelectList(_context.Localization, "LocalizationId", "PlaceName", user.LocalizationId);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,FirstName,MiddleName,LastName,UserType,Address,Number,AddressComplement,Age,Email,Phone,Creditcard,CreditcardName,LocalizationId,InstitutionBranchId")] User user)
        {
            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
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
            ViewData["InstitutionBranchId"] = new SelectList(_context.InstitutionBranch, "InstitutionBranchId", "InstitutionBranchName", user.InstitutionBranchId);
            ViewData["LocalizationId"] = new SelectList(_context.Localization, "LocalizationId", "PlaceName", user.LocalizationId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.InstitutionBranch)
                .Include(u => u.Localization)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
