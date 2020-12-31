using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client/Payments
        public async Task<IActionResult> Index()
        {
            var userId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            var applicationDbContext = _context.tblPayments.Where(c => c.tblPharmacistsId == userId).Include(t => t.tblPharmacists).ThenInclude(c=>c.ApplicationUser).Include(t => t.PaymentType).Include(t => t.Invoice);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Client/Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            var tblPayments = await _context.tblPayments
                .Include(t => t.tblPharmacists)
                .ThenInclude(c => c.ApplicationUser)
                .Include(t => t.PaymentType)
                .Include(t => t.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id && m.tblPharmacistsId == userId);
            if (tblPayments == null)
            {
                return NotFound();
            }

            return View(tblPayments);
        }

        // GET: Client/Payments/Create
        public IActionResult Create()
        {
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "Id");
            return View();
        }

        // POST: Client/Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AmountPaid,PaymentTypeId,PayDate,PaymentComment,Id")] tblPayments tblPayments)
        {
            tblPayments.tblPharmacistsId = Convert.ToInt32(User.FindFirst("ClientId").Value);
            tblPayments.Created = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(tblPayments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "Id", tblPayments.tblPharmacistsId);
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "Id", tblPayments.PaymentTypeId);
            return View(tblPayments);
        }

        // GET: Client/Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPayments = await _context.tblPayments.FindAsync(id);
            if (tblPayments == null)
            {
                return NotFound();
            }
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "Id", tblPayments.tblPharmacistsId);
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "Id", tblPayments.PaymentTypeId);
            return View(tblPayments);
        }

        // POST: Client/Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("tblPharmacistsId,AmountPaid,PaymentTypeId,PayDate,PaymentComment,Id,Created")] tblPayments tblPayments)
        {
            if (id != tblPayments.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPayments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblPaymentsExists(tblPayments.Id))
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
            ViewData["tblPharmacistsId"] = new SelectList(_context.Users, "Id", "Id", tblPayments.tblPharmacistsId);
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "Id", tblPayments.PaymentTypeId);
            return View(tblPayments);
        }

        // GET: Client/Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPayments = await _context.tblPayments
                .Include(t => t.tblPharmacists)
                .Include(t => t.PaymentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPayments == null)
            {
                return NotFound();
            }

            return View(tblPayments);
        }

        // POST: Client/Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblPayments = await _context.tblPayments.FindAsync(id);
            _context.tblPayments.Remove(tblPayments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblPaymentsExists(int id)
        {
            return _context.tblPayments.Any(e => e.Id == id);
        }
    }
}
