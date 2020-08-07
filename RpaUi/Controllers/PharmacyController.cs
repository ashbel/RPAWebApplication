using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Controllers
{
    [Authorize]
    public class PharmacyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PharmacyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pharmacy
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblPharmacies.ToListAsync());
        }

        // GET: Pharmacy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacy = await _context.tblPharmacies
                .FirstOrDefaultAsync(m => m.id == id);
            if (tblPharmacy == null)
            {
                return NotFound();
            }

            return View(tblPharmacy);
        }

        // GET: Pharmacy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Pharmacy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,pharmacyName,pharmacyAddress,pharmacyPhone,pharmacyWebsite")] tblPharmacy tblPharmacy)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblPharmacy.created = local_time;

            if (ModelState.IsValid)
            {
                _context.Add(tblPharmacy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblPharmacy);
        }

        // GET: Pharmacy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacy = await _context.tblPharmacies.FindAsync(id);
            if (tblPharmacy == null)
            {
                return NotFound();
            }
            return View(tblPharmacy);
        }

        // POST: Pharmacy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,created,pharmacyName,pharmacyAddress,pharmacyPhone,pharmacyWebsite")] tblPharmacy tblPharmacy)
        {
            if (id != tblPharmacy.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPharmacy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblPharmacyExists(tblPharmacy.id))
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
            return View(tblPharmacy);
        }

        // GET: Pharmacy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPharmacy = await _context.tblPharmacies
                .FirstOrDefaultAsync(m => m.id == id);
            if (tblPharmacy == null)
            {
                return NotFound();
            }

            return View(tblPharmacy);
        }

        // POST: Pharmacy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblPharmacy = await _context.tblPharmacies.FindAsync(id);
            _context.tblPharmacies.Remove(tblPharmacy);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Upload(IFormCollection formCollection)
        {
            if (Request.Form.Files.Count() > 0)
            {
                string uploadPath = @"wwwroot\Uploads";
                var paymentFileName = "";
                IFormFile file = Request.Form.Files[0];
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(file.FileName));
                paymentFileName = filename;

                var filePath = uploadPath + "/" + filename;

                file.CopyTo(new FileStream(filePath, FileMode.Create));
                

                try
                {
                    StreamReader sr = new StreamReader(file.OpenReadStream());

                    string csvData = sr.ReadToEnd();
                    foreach (string row in csvData.Split('\n'))
                    {
                        String name = "";
                        String address = "";

                        if (!string.IsNullOrEmpty(row))
                        {
                            var values = row.Split(',');
                            name = values[0].ToString().Trim();
                            address = values[1].ToString().Trim();

                            tblPharmacy pharmacy = new tblPharmacy();
                            pharmacy.pharmacyName = name;
                            pharmacy.pharmacyAddress = address;
                            pharmacy.created = DateTime.Now;
                            _context.Add(pharmacy);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception error)
                {
                    string name = error.Message;
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");

        }

        private bool tblPharmacyExists(int id)
        {
            return _context.tblPharmacies.Any(e => e.id == id);
        }
    }
}
