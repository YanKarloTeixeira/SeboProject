using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeboProject.Data;
using SeboProject.Models;

namespace SeboProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly SeboDbContext _context;

        public HomeController(SeboDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var seboDbContext = _context.Book.Include(b => b.BookCondition).Include(b => b.StudyArea).Include(b => b.User);
            return View(await seboDbContext.ToListAsync());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
