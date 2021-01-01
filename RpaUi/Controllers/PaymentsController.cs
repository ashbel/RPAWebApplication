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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Payments
        public async Task<IActionResult> Index()
        {
           
            var applicationDbContext = _context.tblPayments.Include(t => t.tblPharmacists).ThenInclude(c => c.ApplicationUser).Include(t => t.PaymentType).Include(t => t.Invoice);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPayments = await _context.tblPayments
                .Include(t => t.tblPharmacists)
                .ThenInclude(c => c.ApplicationUser)
                .Include(t => t.PaymentType)
                .Include(t => t.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPayments == null)
            {
                return NotFound();
            }

            return View(tblPayments);
        }

        // GET: Payments/Create
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClientId,AmountPaid,PaymentTypeId,PayDate,PaymentComment,Id")] tblPayments tblPayments)
        {
            var local_time = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblPayments.Created =local_time;

            if (ModelState.IsValid)
            {
                _context.Add(tblPayments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName", tblPayments.tblPharmacistsId);
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName", tblPayments.PaymentTypeId);
            return View(tblPayments);
        }

        // GET: Payments/Edit/5
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
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName", tblPayments.tblPharmacistsId);
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName", tblPayments.PaymentTypeId);
            return View(tblPayments);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,AmountPaid,PaymentTypeId,PayDate,PaymentComment,Id,Created")] tblPayments tblPayments)
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
            ViewData["ClientId"] = new SelectList(_context.Users, "Id", "FullName", tblPayments.tblPharmacistsId);
            ViewData["PaymentTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName", tblPayments.PaymentTypeId);
            return View(tblPayments);
        }

        // GET: Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblPayments = await _context.tblPayments
                .Include(t => t.tblPharmacists)
                .ThenInclude(c => c.ApplicationUser)
                .Include(t => t.PaymentType)
                .Include(t => t.Invoice)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblPayments == null)
            {
                return NotFound();
            }

            return View(tblPayments);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblPayments = await _context.tblPayments.FindAsync(id);
            _context.tblPayments.Remove(tblPayments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var tblPayments = await _context.tblPayments.FindAsync(id);
            tblPayments.PaymentStatus = true;
            _context.Update(tblPayments);

            var tblPaymentsClient = await _context.tblInvoiceClient.FirstOrDefaultAsync(c => c.tblInvoicesId == tblPayments.InvoiceId && c.tblPharmacistsId == tblPayments.tblPharmacistsId);
            tblPaymentsClient.paid = true;
            _context.Update(tblPaymentsClient);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id});
        }


        public async Task<IActionResult> UndoConfirmPayment(int id)
        {
            var tblPayments = await _context.tblPayments.FindAsync(id);
            tblPayments.PaymentStatus = false;
            _context.Update(tblPayments);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = id });
        }

        private bool tblPaymentsExists(int id)
        {
            return _context.tblPayments.Any(e => e.Id == id);
        }
    }
}
