using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Interfaces;
using RpaUi.Services;

namespace RpaUi.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> userManager;
        private readonly IMyEmailSender _emailSender;

        public MeetingsController(ApplicationDbContext context,
                                    UserManager<ApplicationUser> usrMgr,
                                    IMyEmailSender emailSender)
        {
            _context = context;
            userManager = usrMgr;
            _emailSender = emailSender;
        }

        // GET: Meetings
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblEvents.ToListAsync());
        }

        // GET: Meetings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Memberships"] = _context.tblMembership;
            var tblEvents = await _context.tblEvents
                .Include(t=>t.tblCertificates)
                .Include(t => t.tblEventsHistory)
                .ThenInclude(t => t.tblPharmacists)
                .ThenInclude(t => t.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblEvents == null)
            {
                return NotFound();
            }

            return View(tblEvents);
        }

        // GET: Meetings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Meetings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventName,EventStartDate,EventEndDate,EventVenue,EventPoints,EventDescription,Id")] tblEvents tblEvents)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblEvents.Created = local_time;

            if (ModelState.IsValid)
            {
                _context.Add(tblEvents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblEvents);
        }

        // GET: Meetings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblEvents = await _context.tblEvents.FindAsync(id);
            if (tblEvents == null)
            {
                return NotFound();
            }
            return View(tblEvents);
        }

        // POST: Meetings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventName,EventStartDate,EventEndDate,EventVenue,EventPoints,EventDescription,Id,Created")] tblEvents tblEvents)
        {
            if (id != tblEvents.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblEvents);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblEventsExists(tblEvents.Id))
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
            return View(tblEvents);
        }

        // GET: Meetings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblEvents = await _context.tblEvents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblEvents == null)
            {
                return NotFound();
            }

            return View(tblEvents);
        }

        // POST: Meetings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblEvents = await _context.tblEvents.FindAsync(id);
            _context.tblEvents.Remove(tblEvents);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SendAsync(int id)
        {
            string[] membershipIds = Request.Form["memberships"].ToArray();
            var eventToInvite = await _context.tblEvents.FindAsync(id);
            var membersInMailingLists = _context.tblMembershipClients.Include(t => t.tblPharmacists)
                .ThenInclude(t => t.ApplicationUser)
                .Where(c => membershipIds.Contains(c.tblMembershipId.ToString())).Distinct().ToList();
            BackgroundJob.Enqueue(() => save_meeting_invites(eventToInvite, membersInMailingLists));
            BackgroundJob.Enqueue(() => send_meeting_invites(eventToInvite, membersInMailingLists));
            return RedirectToAction(nameof(Index));
        }

        public async Task send_meeting_invites(tblEvents eventToInvite, List<tblMembershipClient> members)
        {
            foreach (var member in members)
            {
                await _emailSender.SendEmailAsync(member.tblPharmacists.ApplicationUser.Email, eventToInvite.EventName,
                    $"Dear {member.tblPharmacists.ApplicationUser.FullName} <br>"
                    + "You are invited to attend a Meeting with the following details:<br><br> "
                    + $"<b>Event: </b> {eventToInvite.EventName} <br>"
                    + $"<b>Start: </b> {eventToInvite.EventStartDate:f}  - {eventToInvite.EventEndDate:f}<br>"
                    + $"<b>Venue: </b> {eventToInvite.EventVenue} <br>"
                    + $"<b>Points: </b> {eventToInvite.EventPoints} <br>"
                    + $"<b>Description: </b><br> {eventToInvite.EventName} <br>");
            }
        }

        public async Task save_meeting_invites(tblEvents tblevent, List<tblMembershipClient> members)
        {
            foreach (var member in members)
            {
                var tblEventsHistory = new tblEventsHistory
                {
                    Created = DateTime.Now,
                    Attending = false,
                    AttendedEvent = false,
                    EventId = tblevent.Id,
                    tblPharmacistsId = member.tblPharmacists.Id
                };
                if (!_context.tblEventsHistory.Any(c =>
                    c.tblPharmacistsId == member.tblPharmacists.Id && c.EventId == tblevent.Id))
                {
                    _context.Add(tblEventsHistory);
                }
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> HasAttended(int id, bool act)
        {
            var meeting = await _context.tblEventsHistory.FindAsync(id);
            meeting.AttendedEvent = act;
            _context.Update(meeting);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new {id = meeting.EventId});
        }
        public async Task<IActionResult> GenerateCertificate(int id)
        {
            var meeting_attendees = _context.tblEventsHistory.Include(t => t.Event)
                .Include(t => t.tblPharmacists)
                .ThenInclude(t => t.ApplicationUser)
                .Where(c=>c.EventId == id && c.AttendedEvent);
            foreach (var member in meeting_attendees.ToList())
            {
                var certificate = new tblCertificates
                {
                    tblPharmacistsId = member.tblPharmacistsId,
                    EventId = member.EventId,
                    EventPoints = member.Event.EventPoints,
                    CertificateDate = member.Event.EventEndDate,
                    Created = DateTime.Now
                };
                await _context.tblCertificates.AddAsync(certificate);
                await _context.SaveChangesAsync();

            }
            return RedirectToAction("Details", new {id = id});
        }

        private bool tblEventsExists(int id)
        {
            return _context.tblEvents.Any(e => e.Id == id);
        }
    }
}
