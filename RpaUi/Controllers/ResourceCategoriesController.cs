using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Controllers
{
    public class ResourceCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourceCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ResourceCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblResourceCategory.ToListAsync());
        }

        // GET: ResourceCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResourceCategory = await _context.tblResourceCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblResourceCategory == null)
            {
                return NotFound();
            }

            return View(tblResourceCategory);
        }

        // GET: ResourceCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ResourceCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("catName,catDescription,Id,Created")] tblResourcesCategory tblResourceCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblResourceCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblResourceCategory);
        }

        // GET: ResourceCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResourceCategory = await _context.tblResourceCategory.FindAsync(id);
            if (tblResourceCategory == null)
            {
                return NotFound();
            }
            return View(tblResourceCategory);
        }

        // POST: ResourceCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("catName,catDescription,Id,Created")] tblResourcesCategory tblResourceCategory)
        {
            if (id != tblResourceCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblResourceCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblResourceCategoryExists(tblResourceCategory.Id))
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
            return View(tblResourceCategory);
        }

        // GET: ResourceCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResourceCategory = await _context.tblResourceCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblResourceCategory == null)
            {
                return NotFound();
            }

            return View(tblResourceCategory);
        }

        // POST: ResourceCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblResourceCategory = await _context.tblResourceCategory.FindAsync(id);
            _context.tblResourceCategory.Remove(tblResourceCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblResourceCategoryExists(int id)
        {
            return _context.tblResourceCategory.Any(e => e.Id == id);
        }
    }
}
