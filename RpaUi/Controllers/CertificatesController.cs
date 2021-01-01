using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Utilities;

namespace RpaUi.Controllers
{
    [Authorize]
    public class CertificatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;
        public CertificatesController(ApplicationDbContext context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        // GET: Certificates
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tblCertificates.Include(t => t.tblPharmacists).Include(t => t.Event);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Certificates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCertificates = await _context.tblCertificates
                .Include(t => t.tblPharmacists)
                .Include(t => t.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCertificates == null)
            {
                return NotFound();
            }

            return View(tblCertificates);
        }

        // GET: Certificates/Create
        public IActionResult Create()
        {
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventName");
            return View();
        }

        // POST: Certificates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("tblPharmacistsId,EventId,EventPoints,CertificateDate,Id")] tblCertificates tblCertificates)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblCertificates.Created = local_time;

            if (ModelState.IsValid)
            {
                _context.Add(tblCertificates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["tblPharmacistsId"] = new SelectList(_context.tblPharmacists, "Id", "FullName", tblCertificates.tblPharmacistsId);
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventName", tblCertificates.EventId);
            return View(tblCertificates);
        }

        // GET: Certificates/Edit/5
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
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "FullName", tblCertificates.tblPharmacistsId);
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventName", tblCertificates.EventId);
            return View(tblCertificates);
        }

        // POST: Certificates/Edit/5
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
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "FullName", tblCertificates.tblPharmacistsId);
            ViewData["EventId"] = new SelectList(_context.tblEvents, "Id", "EventName", tblCertificates.EventId);
            return View(tblCertificates);
        }

        // GET: Certificates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCertificates = await _context.tblCertificates
                .Include(t => t.tblPharmacists)
                .Include(t => t.Event)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCertificates == null)
            {
                return NotFound();
            }

            return View(tblCertificates);
        }

        // POST: Certificates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblCertificates = await _context.tblCertificates.FindAsync(id);
            _context.tblCertificates.Remove(tblCertificates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Download(int id)
        {
            var certificate = _context.tblCertificates.Include(t => t.Event)
                .Include(t => t.tblPharmacists).ThenInclude(t => t.ApplicationUser).FirstOrDefault(c => c.Id == id);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(certificate),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "css", "adminlte.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Retail Pharmacists Association" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);
            return File(file, "application/pdf", certificate.Event.EventName + "_Certificate.pdf");
        }
        private bool tblCertificatesExists(int id)
        {
            return _context.tblCertificates.Any(e => e.Id == id);
        }
    }
}
