using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;
using RpaData.Models.ViewModels;
using RpaUi.Interfaces;
using RpaUi.Services;

namespace RpaUi.Controllers
{
    [Authorize]
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> userManager;
        private readonly IMyEmailSender _emailSender;

        public InvoicesController(ApplicationDbContext context, 
                                    UserManager<ApplicationUser> usrMgr,
                                    IMyEmailSender emailSender)
        {
            _context = context;
            userManager = usrMgr;
            _emailSender = emailSender;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tblInvoices.Include(t => t.InvoiceType);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Memberships"] = _context.tblMembership;
            var tblInvoices = await _context.tblInvoices
                .Include(t => t.InvoiceType)
                .Include(t=> t.tblInvoiceClients)
                .ThenInclude(t=>t.tblPharmacists)
                .ThenInclude(t=>t.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblInvoices == null)
            {
                return NotFound();
            }

            return View(tblInvoices);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName");
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Amount,InvoiceTypeId,DueDate,InvoiceComment,Id")] tblInvoices tblInvoices)
        {
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

                tblInvoices.invoiceAttachment = uploadFile;
            }

            tblInvoices.Created = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            
            if (ModelState.IsValid)
            {
                _context.Add(tblInvoices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName", tblInvoices.InvoiceTypeId);
            return View(tblInvoices);
        }

        // GET: Invoices/Edit/5
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
            ViewData["InvoiceTypeId"] = new SelectList(_context.tblCodes, "Id", "CodeName", tblInvoices.InvoiceTypeId);
            return View(tblInvoices);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Amount,InvoiceTypeId,DueDate,InvoiceComment,Id,Created,invoiceAttachment")] tblInvoices tblInvoices)
        {
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

                tblInvoices.invoiceAttachment = uploadFile;
            }

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

        // GET: Invoices/Delete/5
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

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblInvoices = await _context.tblInvoices.FindAsync(id);
            _context.tblInvoices.Remove(tblInvoices);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SendAsync(int id)
        {
            string[] membershipIds = Request.Form["memberships"].ToArray();
            List<string> finalMembers = new List<string>();

            foreach(var item in membershipIds)
            {
                int _id = Convert.ToInt32(item);
                var members = _context.tblMembershipClients.Where(c => c.tblMembershipId == _id);

                foreach(var member in members.ToList())
                {
                    tblInvoicesClient tblInvoiceClient = new tblInvoicesClient();
                    tblInvoiceClient.Created = DateTime.Now;
                    tblInvoiceClient.paid = false;
                    tblInvoiceClient.tblInvoicesId = id;
                    tblInvoiceClient.tblPharmacistsId = member.tblPharmacistsId;
                    if ((_context.tblInvoiceClient.Where(c => c.tblInvoicesId == id && c.tblPharmacistsId == member.tblPharmacistsId)).Count() == 0)
                    {
                        finalMembers.Add(item);
                        _context.Add(tblInvoiceClient);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            BackgroundJob.Enqueue(() => SendEmailJobAsync(id, finalMembers));

            return RedirectToAction(nameof(Index));
        }

        public async Task SendEmailJobAsync(int id, List<string> memberslist = null)
        {
            var tblInvoice = await _context.tblInvoices.FirstOrDefaultAsync(m => m.Id == id);

            List<ApplicationUser> members = new List<ApplicationUser>();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();


            foreach (var member in memberslist)
            {
                var memberId = Convert.ToInt32(memberslist);
                var tblmailinglist = _context.tblMembership.Include(c => c.tblMembershipClients).ThenInclude(c => c.tblPharmacists).SingleOrDefault(x => x.Id == memberId);

                foreach (var mailinglistClient in tblmailinglist.tblMembershipClients)
                {
                    ApplicationUser applicationUser = await userManager.FindByIdAsync(mailinglistClient.tblPharmacists.ApplicationUserId);
                    members.Add(applicationUser);
                }
            }

            foreach (var user in userManager.Users.ToList())
            {
                if (await userManager.IsInRoleAsync(user, "Client"))
                {
                    members.Add(user);
                }
            }

            foreach (var client in members)
            {
                await _emailSender.SendEmailWithAttachmentAsync(client.Email, tblInvoice.InvoiceType.CodeName + " Invoice ", tblInvoice.DueDate + "" + tblInvoice.Amount.ToString("###,##.00"),tblInvoice.invoiceAttachment);

                tblCommunicationLogs tblCommunicationLogs = new tblCommunicationLogs();

                tblCommunicationLogs.ClientId = client.Id;
                tblCommunicationLogs.CommunicationId = id;
                tblCommunicationLogs.Created = DateTime.Now;
                tblCommunicationLogs.Receipient = client.Email;

                _context.Add(tblCommunicationLogs);
                await _context.SaveChangesAsync();

            }
        }

        private bool tblInvoicesExists(int id)
        {
            return _context.tblInvoices.Any(e => e.Id == id);
        }
    }
}
