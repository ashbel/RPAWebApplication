using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Controllers
{
    [Authorize]
    public class CodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Codes
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblCodes.ToListAsync());
        }

        // GET: Codes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCodes = await _context.tblCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCodes == null)
            {
                return NotFound();
            }

            return View(tblCodes);
        }

        // GET: Codes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Codes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CodeName,CodeEntity,Id")] tblCodes tblCodes)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblCodes.Created = local_time;

            if (ModelState.IsValid)
            {
                _context.Add(tblCodes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblCodes);
        }

        // GET: Codes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCodes = await _context.tblCodes.FindAsync(id);
            if (tblCodes == null)
            {
                return NotFound();
            }
            return View(tblCodes);
        }

        // POST: Codes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CodeName,CodeEntity,Id,Created")] tblCodes tblCodes)
        {
            if (id != tblCodes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCodes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblCodesExists(tblCodes.Id))
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
            return View(tblCodes);
        }

        // GET: Codes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCodes = await _context.tblCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCodes == null)
            {
                return NotFound();
            }

            return View(tblCodes);
        }

        // POST: Codes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblCodes = await _context.tblCodes.FindAsync(id);
            _context.tblCodes.Remove(tblCodes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblCodesExists(int id)
        {
            return _context.tblCodes.Any(e => e.Id == id);
        }
    }
}
