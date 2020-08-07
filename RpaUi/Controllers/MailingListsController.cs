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
    public class MailingListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MailingListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MailingLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblMailingList.Include(t=>t.tblMailingListClients).ToListAsync());
        }

        // GET: MailingLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clientsInList = _context.tblMailingListClients.Where(c => c.tblMailingListId == id).Select(c=>c.tblPharmacistId);

            var clients = from c in _context.tblPharmacists.Include(t => t.Client)
                          where !(clientsInList.Contains(c.Id))
                          select c;

            ViewBag.Clients = clients;

            var tblMailingList = await _context.tblMailingList
                .Include(t => t.tblMailingListClients).ThenInclude(c=>c.tblPharmacist).ThenInclude(c=>c.Client)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblMailingList == null)
            {
                return NotFound();
            }

            return View(tblMailingList);
        }

        // GET: MailingLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MailingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ListName,ListDescription,Id,Created")] tblMailingList tblMailingList)
        {
            tblMailingList.Created = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(tblMailingList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblMailingList);
        }

        // GET: MailingLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMailingList = await _context.tblMailingList.FindAsync(id);
            if (tblMailingList == null)
            {
                return NotFound();
            }
            return View(tblMailingList);
        }

        // POST: MailingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ListName,ListDescription,Id,Created")] tblMailingList tblMailingList)
        {
            if (id != tblMailingList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblMailingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblMailingListExists(tblMailingList.Id))
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
            return View(tblMailingList);
        }

        // GET: MailingLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblMailingList = await _context.tblMailingList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblMailingList == null)
            {
                return NotFound();
            }

            return View(tblMailingList);
        }

        // POST: MailingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblMailingList = await _context.tblMailingList.FindAsync(id);
            _context.tblMailingList.Remove(tblMailingList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        

        public async Task<IActionResult> Add(int id, int pharmaId)
        {
            tblMailingListClients tblMailingListClients = new tblMailingListClients();

            tblMailingListClients.tblMailingListId = id;
            tblMailingListClients.tblPharmacistId = pharmaId;
            tblMailingListClients.Created = DateTime.Now;
            
            _context.Add(tblMailingListClients);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id});
        }

        public async Task<IActionResult> Remove(int id, int mailId)
        {
            var tblMailingListClients = await _context.tblMailingListClients.FindAsync(id);
            _context.tblMailingListClients.Remove(tblMailingListClients);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = mailId });
        }

        private bool tblMailingListExists(int id)
        {
            return _context.tblMailingList.Any(e => e.Id == id);
        }
    }
}
