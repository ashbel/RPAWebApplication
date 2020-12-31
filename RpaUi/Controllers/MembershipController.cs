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
    public class MembershipController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembershipController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Membership
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblMembership.Include(o=>o.tblMembershipClients).ToListAsync());
        }

        // GET: Membership/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientsInList = _context.tblMembershipClients.Where(c => c.tblMembershipId == id).Select(c => c.tblPharmacistsId);

            var clients = from c in _context.tblPharmacists.Include(t => t.ApplicationUser)
                          where !(clientsInList.Contains(c.Id))
                          select c;

            ViewBag.Clients = clients;

            var tblMembership = await _context.tblMembership
                 .Include(t => t.tblMembershipClients).ThenInclude(c => c.tblPharmacists).ThenInclude(c => c.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblMembership == null)
            {
                return NotFound();
            }

            return View(tblMembership);
        }

        // GET: Membership/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Membership/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("name,description,Id,Created")] tblMembership tblMembership)
        {
            tblMembership.Created = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(tblMembership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblMembership);
        }

        // GET: Membership/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMembership = await _context.tblMembership.FindAsync(id);
            if (tblMembership == null)
            {
                return NotFound();
            }
            return View(tblMembership);
        }

        // POST: Membership/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("name,description,Id,Created")] tblMembership tblMembership)
        {
            if (id != tblMembership.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblMembership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblMembershipExists(tblMembership.Id))
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
            return View(tblMembership);
        }

        // GET: Membership/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMembership = await _context.tblMembership
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblMembership == null)
            {
                return NotFound();
            }

            return View(tblMembership);
        }

        // POST: Membership/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblMembership = await _context.tblMembership.FindAsync(id);
            _context.tblMembership.Remove(tblMembership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Add(int id, int pharmaId)
        {
            tblMembershipClient tblMembershipClient = new tblMembershipClient();

            tblMembershipClient.tblMembershipId = id;
            tblMembershipClient.tblPharmacistsId = pharmaId;
            tblMembershipClient.Created = DateTime.Now;

            _context.Add(tblMembershipClient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> Remove(int id, int itemId)
        {
            var tblMembershipClient = await _context.tblMembershipClients.FindAsync(id);
            _context.tblMembershipClients.Remove(tblMembershipClient);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = itemId });
        }

        private bool tblMailingListExists(int id)
        {
            return _context.tblMailingList.Any(e => e.Id == id);
        }
        private bool tblMembershipExists(int id)
        {
            return _context.tblMembership.Any(e => e.Id == id);
        }
    }
}
