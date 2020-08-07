using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Services;

namespace RpaUi.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMyEmailSender _emailSender;

        public JobsController(ApplicationDbContext context, IMyEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: Jobs
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblJobs.ToListAsync());
        }

        // GET: Jobs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblJobs = await _context.tblJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblJobs == null)
            {
                return NotFound();
            }

            return View(tblJobs);
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobName,JobStatus,JobFrequency,Id,Created")] tblJobs tblJobs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblJobs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblJobs);
        }

        // GET: Jobs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblJobs = await _context.tblJobs.FindAsync(id);
            if (tblJobs == null)
            {
                return NotFound();
            }
            return View(tblJobs);
        }

        // POST: Jobs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobName,JobStatus,JobFrequency,Id,Created")] tblJobs tblJobs)
        {
            if (id != tblJobs.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblJobs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblJobsExists(tblJobs.Id))
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
            return View(tblJobs);
        }

        // GET: Jobs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblJobs = await _context.tblJobs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblJobs == null)
            {
                return NotFound();
            }

            return View(tblJobs);
        }

        // POST: Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblJobs = await _context.tblJobs.FindAsync(id);
            _context.tblJobs.Remove(tblJobs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ActivateJob()
        {
            RecurringJob.AddOrUpdate(() => BirthdayMessageJobAsync(), "0 2 * * *");
            RecurringJob.AddOrUpdate(() => MeetingReminderJobAsync(), "0 2 * * *");
            RecurringJob.AddOrUpdate(() => MembershipRenewalOneDayJobAsync(), "0 2 * * *");
            RecurringJob.AddOrUpdate(() => MembershipRenewalSevenDaysJobAsync(), "0 2 * * *");

            return RedirectToAction(nameof(Index));
        }

        public async Task BirthdayMessageJobAsync()
        {
            var clients = _context.tblPharmacists.Include(t => t.Client).Where(c=>c.Client.DateOfBirth.Date == DateTime.Now.Date);

            foreach(var client in clients)
            {
                await _emailSender.SendEmailAsync(client.Client.Email, "Birthday Message", "Dear "+ client.Client.FullName + "<p>We would like to wish you a Happy Birthday.</p> Regards <p>Retail Pharmacists Asscociation</p>");
            }
        }

        public async Task MembershipRenewalSevenDaysJobAsync()
        {
            var dateSevenDays = DateTime.Now.AddDays(7);
            var clients = _context.tblPharmacists.Include(t => t.Client).Where(c => c.dateOfRenewal.Date == dateSevenDays);

            foreach (var client in clients)
            {
                await _emailSender.SendEmailAsync(client.Client.Email, "Membership Renewal", "Dear " + client.Client.FullName + "<p>We would like to remind you that your membership payment will be due in the next 7 days.</p> Regards <p>Retail Pharmacists Asscociation</p>");
            }
        }

        public async Task MembershipRenewalOneDayJobAsync()
        {
            var dateOneDay = DateTime.Now.AddDays(1);
            var clients = _context.tblPharmacists.Include(t => t.Client).Where(c => c.dateOfRenewal.Date == dateOneDay);

            foreach (var client in clients)
            {
                await _emailSender.SendEmailAsync(client.Client.Email, "Membership Renewal", "Dear " + client.Client.FullName + "<p>We would like to remind you that your membership payment is now due.</p> Regards <p>Retail Pharmacists Asscociation</p>");
            }
        }

        public async Task MeetingReminderJobAsync()
        {
            var dateOneDay = DateTime.Now.AddDays(1);
            var clients = _context.tblEventsHistory.Include(c=>c.Client).Include(c=>c.Event).Where(c => c.Event.EventStartDate.Date == dateOneDay);

            foreach (var client in clients)
            {
                await _emailSender.SendEmailAsync(client.Client.Email, "Meeting Reminder", "Dear " + client.Client.FullName + "<p>We would like to remind you of the "+ client.Event.EventName+" meeting.</p> Regards <p>Retail Pharmacists Asscociation</p>");
            }
        }

        private bool tblJobsExists(int id)
        {
            return _context.tblJobs.Any(e => e.Id == id);
        }
    }
}
