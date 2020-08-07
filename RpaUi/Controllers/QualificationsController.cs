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
    public class QualificationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public QualificationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Qualifications
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblQualifications.ToListAsync());
        }

        // GET: Qualifications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblQualifications = await _context.tblQualifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblQualifications == null)
            {
                return NotFound();
            }

            return View(tblQualifications);
        }

        // GET: Qualifications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Qualifications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("qName,qDescription,Id")] tblQualifications tblQualifications)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblQualifications.Created = local_time;

            if (ModelState.IsValid)
            {
                _context.Add(tblQualifications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblQualifications);
        }

        // GET: Qualifications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblQualifications = await _context.tblQualifications.FindAsync(id);
            if (tblQualifications == null)
            {
                return NotFound();
            }
            return View(tblQualifications);
        }

        // POST: Qualifications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("qName,qDescription,Id,Created")] tblQualifications tblQualifications)
        {
            if (id != tblQualifications.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblQualifications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblQualificationsExists(tblQualifications.Id))
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
            return View(tblQualifications);
        }

        // GET: Qualifications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblQualifications = await _context.tblQualifications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblQualifications == null)
            {
                return NotFound();
            }

            return View(tblQualifications);
        }

        // POST: Qualifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblQualifications = await _context.tblQualifications.FindAsync(id);
            _context.tblQualifications.Remove(tblQualifications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblQualificationsExists(int id)
        {
            return _context.tblQualifications.Any(e => e.Id == id);
        }
    }
}
