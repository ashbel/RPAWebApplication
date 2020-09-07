using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Areas.Client.Controllers
{
    [Area("Client")]
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client/Resources
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblResources.ToListAsync());
        }

        // GET: Client/Resources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResources = await _context.tblResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblResources == null)
            {
                return NotFound();
            }

            return View(tblResources);
        }

        // GET: Client/Resources/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Client/Resources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResourceName,ResourceDescription,FileName,Id,Created")] tblResources tblResources)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblResources);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblResources);
        }

        // GET: Client/Resources/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResources = await _context.tblResources.FindAsync(id);
            if (tblResources == null)
            {
                return NotFound();
            }
            return View(tblResources);
        }

        // POST: Client/Resources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResourceName,ResourceDescription,FileName,Id,Created")] tblResources tblResources)
        {
            if (id != tblResources.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblResources);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblResourcesExists(tblResources.Id))
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
            return View(tblResources);
        }

        // GET: Client/Resources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResources = await _context.tblResources
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblResources == null)
            {
                return NotFound();
            }

            return View(tblResources);
        }

        // POST: Client/Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblResources = await _context.tblResources.FindAsync(id);
            _context.tblResources.Remove(tblResources);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblResourcesExists(int id)
        {
            return _context.tblResources.Any(e => e.Id == id);
        }
    }
}
