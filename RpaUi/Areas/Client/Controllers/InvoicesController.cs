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

namespace RpaUi.Areas.Client.Controllers
{
    [Authorize]
    [Area("Client")]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Client/Invoices
        public async Task<IActionResult> Index()
        {
            var clientId = User.FindFirst("ClientId").Value;
            var applicationDbContext = _context.tblInvoices.Include(t => t.InvoiceType).Include(t=>t.tblPayments);
            var payments = _context.tblPayments.Include(t => t.Invoice).Where(c => c.ClientId == clientId);
            ViewBag.Payments = payments;
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Client/Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblInvoices = await _context.tblInvoices
                .Include(t => t.tblPayments)
                .Include(t => t.InvoiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblInvoices == null)
            {
                return NotFound();
            }

            return View(tblInvoices);
        }

        // GET: Client/Invoices/Create
        public IActionResult Create()
        {
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "Id");
            return View();
        }

        // POST: Client/Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,InvoiceTypeId,DueDate,InvoiceComment,Id,Created")] tblInvoices tblInvoices)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblInvoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "Id", tblInvoices.InvoiceTypeId);
            return View(tblInvoices);
        }

        // GET: Client/Invoices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblInvoices = await _context.tblInvoices.FindAsync(id);
            if (tblInvoices == null)
            {
                return NotFound();
            }
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "Id", tblInvoices.InvoiceTypeId);
            return View(tblInvoices);
        }

        // POST: Client/Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Amount,InvoiceTypeId,DueDate,InvoiceComment,Id,Created")] tblInvoices tblInvoices)
        {
            if (id != tblInvoices.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblInvoices);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblInvoicesExists(tblInvoices.Id))
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
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "Id", tblInvoices.InvoiceTypeId);
            return View(tblInvoices);
        }

        // GET: Client/Invoices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblInvoices = await _context.tblInvoices
                .Include(t => t.InvoiceType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblInvoices == null)
            {
                return NotFound();
            }

            return View(tblInvoices);
        }

        // POST: Client/Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblInvoices = await _context.tblInvoices.FindAsync(id);
            _context.tblInvoices.Remove(tblInvoices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Payment(IFormCollection formCollection)
        {
            var Id = Convert.ToInt32(formCollection["Id"]);
            var amountPaid = formCollection["amount_due"];
            var paymentDetails = formCollection["payment_details"];
            var paymentDate = formCollection["payment_date"];
            var paymentTypeId = Convert.ToInt32(formCollection["payment_type_id"]);
            var paymentFileName = "";

            if (Request.Form.Files.Count() > 0)
            {
                IFormFile file = Request.Form.Files[0];
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(file.FileName));
                paymentFileName = filename;

                string uploadPath = @"wwwroot\Uploads";
                if (!Directory.Exists(Path.Combine(uploadPath)))
                {
                    // If it doesn't exist, create the directory
                    Directory.CreateDirectory(Path.Combine(uploadPath));
                }
                var filePath = uploadPath + "/" + filename;

                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));
            }

            var tblInvoices = await _context.tblInvoices.FindAsync(Id);

            tblPayments payments = new tblPayments();
            payments.AmountPaid = Convert.ToDecimal(amountPaid);
            payments.ClientId = User.FindFirst("UserId").Value;
            payments.PayDate = Convert.ToDateTime(paymentDate);
            payments.PaymentComment = paymentDetails;
            payments.Created = DateTime.Now;
            payments.PaymentTypeId = paymentTypeId;
            payments.ProofOfPayment = paymentFileName;
            payments.InvoiceId = Id;

            _context.Add(payments);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool tblInvoicesExists(int id)
        {
            return _context.tblInvoices.Any(e => e.Id == id);
        }
    }
}
