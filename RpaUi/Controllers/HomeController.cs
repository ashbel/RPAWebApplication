using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RpaData.DataContext;
using RpaUi.Models;

namespace RpaUi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
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

        public IActionResult Home()
        {
            ViewBag.Client = _context.tblPharmacists.Count();
            ViewBag.ClientsMonth = _context.tblPharmacists.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Count();
            ViewBag.Vehicles = _context.tblEvents.Count();
            ViewBag.VehiclesMonth = _context.tblEvents.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Count();
            ViewBag.WashesCount = _context.tblPayments.Count();
            ViewBag.WashesCountMonth = _context.tblPayments.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Count();
            ViewBag.Washes = _context.tblPayments.Sum(c => c.AmountPaid).ToString("$#,##0.00");
            ViewBag.WashesMonth = _context.tblPayments.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Sum(c => c.AmountPaid).ToString("$#,##0.00");

            return View();
        }
    }
}
