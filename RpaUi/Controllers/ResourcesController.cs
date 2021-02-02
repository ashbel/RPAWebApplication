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
    public class ResourcesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResourcesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resources
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tblResources.Include(t => t.tblResourceCategory).Include(t=>t.tblResourcesMembers);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Resources/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var memebersInList = _context.tblResourcesMembers.Where(c => c.tblResourcesId == id).Select(c => c.tblMembershipId);

            var memberships = from c in _context.tblMembership.Include(t => t.tblResourcesMembers)
                          where !(memebersInList.Contains(c.Id))
                          select c;

            ViewBag.Membership = memberships;

            var tblResources = await _context.tblResources
                .Include(t => t.tblResourceCategory)
                .Include(t=>t.tblResourcesMembers).ThenInclude(t=>t.tblMembership)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblResources == null)
            {
                return NotFound();
            }

            return View(tblResources);
        }

        // GET: Resources/Create
        public IActionResult Create()
        {
            ViewData["tblResourceCategoryId"] = new SelectList(_context.Set<tblResourcesCategory>(), "Id", "catName");
            return View();
        }

        // POST: Resources/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResourceName,tblResourceCategoryId,ResourceDescription,FileName,Id,Created")] tblResources tblResources)
        {
            tblResources.Created = DateTime.Now;
            tblResources.Guid = Guid.NewGuid().ToString();
            var uploadFile = "";
            if (Request.Form.Files.Count() > 0)
            {
                IFormFile file = Request.Form.Files[0];
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(file.FileName));
                uploadFile = filename;

                string uploadPath = @"wwwroot\Uploads";
                if (!Directory.Exists(Path.Combine(uploadPath)))
                {
                    // If it doesn't exist, create the directory
                    Directory.CreateDirectory(Path.Combine(uploadPath));
                }
                var filePath = uploadPath + "/" + filename;

                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                tblResources.FileName = uploadFile;
            }

            if (ModelState.IsValid)
            {
                _context.Add(tblResources);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["tblResourceCategoryId"] = new SelectList(_context.Set<tblResourcesCategory>(), "Id", "catName", tblResources.tblResourceCategoryId);
            return View(tblResources);
        }

        // GET: Resources/Edit/5
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
            ViewData["tblResourceCategoryId"] = new SelectList(_context.Set<tblResourcesCategory>(), "Id", "catName", tblResources.tblResourceCategoryId);
            return View(tblResources);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ResourceName,tblResourceCategoryId,ResourceDescription,FileName,Id,Created,Guid")] tblResources tblResources)
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
            ViewData["tblResourceCategoryId"] = new SelectList(_context.Set<tblResourcesCategory>(), "Id", "catName", tblResources.tblResourceCategoryId);
            return View(tblResources);
        }

        // GET: Resources/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblResources = await _context.tblResources
                .Include(t => t.tblResourceCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblResources == null)
            {
                return NotFound();
            }

            return View(tblResources);
        }

        // POST: Resources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblResources = await _context.tblResources.FindAsync(id);
            _context.tblResources.Remove(tblResources);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Add(int id, int tblMembershipId)
        {
            tblResourcesMember tblResourcesMember = new tblResourcesMember();
            tblResourcesMember.Created = DateTime.Now;
            tblResourcesMember.tblMembershipId = tblMembershipId;
            tblResourcesMember.tblResourcesId = id;

            _context.Add(tblResourcesMember);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> Remove(int id, int resourceId)
        {
            var tblResourcesMember = await _context.tblResourcesMembers.FindAsync(id);
            _context.tblResourcesMembers.Remove(tblResourcesMember);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = resourceId });
        }

        private bool tblResourcesExists(int id)
        {
            return _context.tblResources.Any(e => e.Id == id);
        }
    }
}
