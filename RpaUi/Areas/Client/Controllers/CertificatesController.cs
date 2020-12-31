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
    public class CertificatesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CertificatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client/Certificates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tblCertificates.Include(t => t.Event).Include(t => t.tblPharmacists);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Client/Certificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCertificates = await _context.tblCertificates
                .Include(t => t.Event)
                .Include(t => t.tblPharmacists)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCertificates == null)
            {
                return NotFound();
            }

            return View(tblCertificates);
        }

        // GET: Client/Certificates/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventDescription");
            ViewData["tblPharmacistsId"] = new SelectList(_context.tblPharmacists, "Id", "ApplicationUserId");
            return View();
        }

        // POST: Client/Certificates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tblPharmacistsId,EventId,EventPoints,CertificateDate,Id,Created")] tblCertificates tblCertificates)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblCertificates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventDescription", tblCertificates.EventId);
            ViewData["tblPharmacistsId"] = new SelectList(_context.tblPharmacists, "Id", "ApplicationUserId", tblCertificates.tblPharmacistsId);
            return View(tblCertificates);
        }

        // GET: Client/Certificates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCertificates = await _context.tblCertificates.FindAsync(id);
            if (tblCertificates == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventDescription", tblCertificates.EventId);
            ViewData["tblPharmacistsId"] = new SelectList(_context.tblPharmacists, "Id", "ApplicationUserId", tblCertificates.tblPharmacistsId);
            return View(tblCertificates);
        }

        // POST: Client/Certificates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("tblPharmacistsId,EventId,EventPoints,CertificateDate,Id,Created")] tblCertificates tblCertificates)
        {
            if (id != tblCertificates.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCertificates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblCertificatesExists(tblCertificates.Id))
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
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventDescription", tblCertificates.EventId);
            ViewData["tblPharmacistsId"] = new SelectList(_context.tblPharmacists, "Id", "ApplicationUserId", tblCertificates.tblPharmacistsId);
            return View(tblCertificates);
        }

        // GET: Client/Certificates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCertificates = await _context.tblCertificates
                .Include(t => t.Event)
                .Include(t => t.tblPharmacists)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCertificates == null)
            {
                return NotFound();
            }

            return View(tblCertificates);
        }

        // POST: Client/Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblCertificates = await _context.tblCertificates.FindAsync(id);
            _context.tblCertificates.Remove(tblCertificates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblCertificatesExists(int id)
        {
            return _context.tblCertificates.Any(e => e.Id == id);
        }
    }
}
