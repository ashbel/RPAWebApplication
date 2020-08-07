using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RpaUi.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    public class HomeController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(UserManager<ApplicationUser> usrManager , ApplicationDbContext appDbContext)
        {
            userManager = usrManager;
            _context = appDbContext;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var clientId = User.FindFirst("ClientId").Value;
            var userId = User.FindFirst("UserId").Value;

            var profileComplete = await _context.tblPharmacists.FirstOrDefaultAsync(c => c.ClientId == userId);
            ViewBag.RenewalDate = profileComplete.dateOfRenewal.ToString("dd MMM yyyy");
            ViewBag.AccountStatus = profileComplete.status ? "Paid Up" : "In Arrears";

            ViewBag.Client = await _context.tblPharmacists.CountAsync();
            ViewBag.ClientsMonth = await _context.tblPharmacists.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).CountAsync();
            ViewBag.Meetings_Year = await _context.tblEvents.CountAsync();
            ViewBag.Meetings_This_Month = await _context.tblEvents.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).CountAsync();
            ViewBag.Payments_Year_Count = await _context.tblPayments.CountAsync();
            ViewBag.Payments_This_Month_Count = await _context.tblPayments.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).CountAsync();

            ViewBag.Payments_Year =  _context.tblPayments.Sum(c => c.AmountPaid).ToString("$#,##0.00");
            ViewBag.Payments_This_Month =  _context.tblPayments.Where(c => c.Created.Year == DateTime.Now.Year && c.Created.Month == DateTime.Now.Month).Sum(c => c.AmountPaid).ToString("$#,##0.00");

            if (profileComplete.profileComplete)
            {
                return RedirectToAction("Index", "Profile", new { id = clientId });
            }

            return View();
        }
    }
}