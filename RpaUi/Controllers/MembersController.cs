using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Interfaces;
using RpaUi.Services;

namespace RpaUi.Controllers
{
    [Authorize]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMyEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        public MembersController(UserManager<ApplicationUser> userManager, IMyEmailSender emailSender, ApplicationDbContext context)
        {
            _context = context;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tblPharmacists.Include(t => t.ApplicationUser)
                                        .Include(t => t.tblPharmacy)
                                        .Include(t => t.tblMailingListClients).ThenInclude(c => c.tblMailingList)
                                        .Include(t => t.tblMembershipClients).ThenInclude(c => c.tblMembership)
                                        .Include(t => t.tblInvoiceClients).ThenInclude(c => c.tblInvoices).ThenInclude(c=>c.InvoiceType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists
                .Include(t => t.ApplicationUser)
                .Include(t => t.tblPharmacy)
                .Include(t => t.tblMailingListClients).ThenInclude(c => c.tblMailingList)
                .Include(t => t.tblMembershipClients).ThenInclude(c => c.tblMembership)
                .Include(t => t.tblInvoiceClients).ThenInclude(c => c.tblInvoices)
                .ThenInclude(c => c.InvoiceType)
                .Include(t => t.tblEventsHistory).ThenInclude(c => c.Event)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tblPharmacists == null)
            {
                return NotFound();
            }
            ViewBag.Qualifications = _context.tblQualifications_Pharmacists.Include(t => t.Qualification).Where(c => c.PharmacistId == id);
            return View(tblPharmacists);
        }

        // GET: Clients/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyName");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,tblPharmacyid,workAddress,qualifications,yearsInPractice,dateOfJoiningRPA,practiceNumber,goodStandingPCZ,goodStandingReasonPCZ,goodStandingMCAZ,goodStandingReasonMCAZ,Id")] tblPharmacists tblPharmacists)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblPharmacists.Created =local_time;
            if (ModelState.IsValid)
            {
                _context.Add(tblPharmacists);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyName", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyName", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,tblPharmacyid,workAddress,qualifications,yearsInPractice,dateOfJoiningRPA,practiceNumber,goodStandingPCZ,goodStandingReasonPCZ,goodStandingMCAZ,goodStandingReasonMCAZ,Id,Created")] tblPharmacists tblPharmacists)
        {
            if (id != tblPharmacists.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPharmacists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblPharmacistsExists(tblPharmacists.Id))
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
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName", tblPharmacists.ApplicationUserId);
            ViewData["tblPharmacyid"] = new SelectList(_context.tblPharmacies, "id", "pharmacyName", tblPharmacists.tblPharmacyid);
            return View(tblPharmacists);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacists = await _context.tblPharmacists
                .Include(t => t.ApplicationUser)
                .Include(t => t.tblPharmacy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPharmacists == null)
            {
                return NotFound();
            }

            return View(tblPharmacists);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblPharmacists = await _context.tblPharmacists.FindAsync(id);
            _context.tblPharmacists.Remove(tblPharmacists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Confirm(int id, bool act)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            var tblPharmacists = await _context.tblPharmacists.Include(c=>c.ApplicationUser).FirstOrDefaultAsync(c=>c.Id == id);
            tblPharmacists.status = act;
            string returnUrl = null;


            try
            {
                if (act)
                {
                    tblPharmacists.dateOfRenewal = local_time.AddYears(1);
                    var user = await _userManager.FindByNameAsync(tblPharmacists.ApplicationUser.UserName);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                                                "/Account/ConfirmEmail",
                                                pageHandler: null,
                                                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                                                protocol: Request.Scheme);

                    var subject = "Confirm your email";
                    var emailBody = $"<p> Dear <b>" + user.FullName + $"</b> </p>  Your Account has been approved. </br> Please confirm your account by" +
                                        $" <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
                    BackgroundJob.Enqueue(() => SendEmailJobAsync(user, subject, emailBody));

                    if ((_context.tblMailingListClients.Where(c => c.tblPharmacistId == id)).Count() == 0)
                    {
                        tblMailingListClients tblMailingListClients = new tblMailingListClients();
                        tblMailingListClients.tblMailingListId = _context.tblMailingList.FirstOrDefault(c => c.ListName == "All Members").Id;
                        tblMailingListClients.tblPharmacistId = id;
                        tblMailingListClients.Created = DateTime.Now;
                        _context.Add(tblMailingListClients);
                    }

                    if ((_context.tblMembershipClients.Where(c => c.tblPharmacistsId == id)).Count() == 0)
                    {
                        tblMembershipClient tblMembershipClient = new tblMembershipClient();
                        tblMembershipClient.tblMembershipId = _context.tblMembership.FirstOrDefault(c => c.name == "Default Membership").Id;
                        tblMembershipClient.tblPharmacistsId = id;
                        tblMembershipClient.Created = DateTime.Now;
                        _context.Add(tblMembershipClient);
                    }

                }
                else
                {
                    tblPharmacists.dateOfRenewal = local_time.AddYears(1);
                    var user = await _userManager.FindByNameAsync(tblPharmacists.ApplicationUser.UserName);
                    user.EmailConfirmed = false;
                    await _userManager.UpdateAsync(user);

                    var subject = "Account Declined/Suspended";
                    var emailBody = $"<p> Dear <b>" + user.FullName + $"</b> </p>  Your Account has been declined/suspended. </br> If you think this was done in error please email secretary@rpa.co.zw";
                    BackgroundJob.Enqueue(() => SendEmailJobAsync(user, subject, emailBody));
                }
            

                _context.Update(tblPharmacists);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tblPharmacistsExists(tblPharmacists.Id))
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

        public async Task SendEmailJobAsync(ApplicationUser user, string subject,string body)
        {
            
            await _emailSender.SendEmailAsync(user.Email, subject, body);
        }

        private bool tblPharmacistsExists(int id)
        {
            return _context.tblPharmacists.Any(e => e.Id == id);
        }
    }
}
