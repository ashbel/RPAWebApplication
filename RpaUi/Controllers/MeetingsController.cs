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

namespace RpaUi.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MeetingsController(ApplicationDbContext context)
        {
            _context = context;
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

            var tblEvents = await _context.tblEvents
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

        private bool tblEventsExists(int id)
        {
            return _context.tblEvents.Any(e => e.Id == id);
        }
    }
}
