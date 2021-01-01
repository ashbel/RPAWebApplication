using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaUi.Utilities;

namespace RpaUi.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    public class CertificatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConverter _converter;
        private readonly IWebHostEnvironment _environment;
        public CertificatesController(ApplicationDbContext context,IConverter converter, IWebHostEnvironment environment)
        {
            _context = context;
            _converter = converter;
            _environment = environment;
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


        [HttpGet]
        public IActionResult Download(int id)
        {
            var certificate = _context.tblCertificates.Include(t => t.Event)
                .Include(t => t.tblPharmacists).ThenInclude(t => t.ApplicationUser).FirstOrDefault(c => c.Id == id);
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A5,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = TemplateGenerator.GetHTMLString(certificate),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(_environment.WebRootPath, "css", "adminlte.css") },
                HeaderSettings = {  Center = "Retail Pharmacists Association", Line = true, FontName = "Calibri", FontSize = 9},
                FooterSettings = { Line = true, Center = "Retail Pharmacists Association", FontName = "Calibri", FontSize = 9 }
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
