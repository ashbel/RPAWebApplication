using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        //var clientId = User.FindFirst("ClientId").Value;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client/Events
        public async Task<IActionResult> Index()
        {
            
            ViewBag.Greeting = "Hello";
            var clientId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            return View(await _context.tblEventsHistory.Include(t=>t.Event).Where(c=>c.tblPharmacistsId == clientId).ToListAsync());
        }

        // GET: Client/Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var clientId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            if (id == null)
            {
                return NotFound();
            }

            var tblEvents = await _context.tblEventsHistory.Include(t=>t.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            var eventAttendance = await _context.tblEventsHistory.AnyAsync(c => c.tblPharmacistsId == clientId && c.EventId == id);
            if (tblEvents == null)
            {
                return NotFound();
            }

            return View(tblEvents);
        }

        // GET: Client/Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventName,EventStartDate,EventEndDate,EventVenue,EventPoints,EventDescription,Id,Created")] tblEvents tblEvents)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblEvents);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblEvents);
        }

        // GET: Client/Events/Edit/5
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

        // POST: Client/Events/Edit/5
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

        // GET: Client/Events/Delete/5
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

        // POST: Client/Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblEvents = await _context.tblEvents.FindAsync(id);
            _context.tblEvents.Remove(tblEvents);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Confirm(int id, bool attend)
        {
            var tblEvents = await _context.tblEvents.FindAsync(id);
            var clientId = Convert.ToInt32(User.FindFirst("ClientId").Value);

            if (tblEventsHistoryExists(id, clientId))
            {
                tblEventsHistory tblEventsHistory = await _context.tblEventsHistory.FirstOrDefaultAsync(c => c.tblPharmacistsId == clientId && c.EventId == id);
                tblEventsHistory.Attending = attend;
                _context.Update(tblEventsHistory);
                await _context.SaveChangesAsync();
            }
            else
            {
                tblEventsHistory tblEventsHistory = new tblEventsHistory();
                tblEventsHistory.tblPharmacistsId = clientId;
                tblEventsHistory.EventId = id;
                tblEventsHistory.Attending = attend;
                tblEventsHistory.Created = DateTime.Now;

                _context.Add(tblEventsHistory);
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblEventsExists(int id)
        {
            return _context.tblEvents.Any(e => e.Id == id);
        }

        private bool tblEventsHistoryExists(int meetingId, int clientId)
        {
            return _context.tblEventsHistory.Any(e => e.tblPharmacistsId == clientId && e.EventId == meetingId);
        }
    }
}
