using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public class CommunicationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly IMyEmailSender _emailSender;

        public CommunicationsController(ApplicationDbContext context,
                                             UserManager<ApplicationUser> usrMgr,
                                RoleManager<IdentityRole> roleMgr, IMyEmailSender emailSender)
        {
            _context = context;
            userManager = usrMgr;
            roleManager = roleMgr;
            _emailSender = emailSender;
        }
        // GET: Communications
        public async Task<IActionResult> Index()
        {
            return View(await _context.tblCommunications.ToListAsync());
        }

        // GET: Communications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["MailingList"] = _context.tblMailingList;

            var tblCommunication = await _context.tblCommunications
                .FirstOrDefaultAsync(m => m.Id == id);
            CommunicationViewModel communicationViewModel = new CommunicationViewModel();
            communicationViewModel.Id = tblCommunication.Id;
            communicationViewModel.communicationAttachment = tblCommunication.communicationAttachment;
            communicationViewModel.Created = tblCommunication.Created;
            communicationViewModel.emailsent = tblCommunication.emailsent;
            communicationViewModel.fileName = tblCommunication.fileName;
            communicationViewModel.Message = tblCommunication.Message;
            communicationViewModel.MessageDate = tblCommunication.MessageDate;
            communicationViewModel.Subject = tblCommunication.Subject;
            communicationViewModel.tblCommunicationLogs = _context.tblCommunicationLogs.Include(c=>c.Client).Where(c => c.CommunicationId == id).ToList();
            
            if (tblCommunication == null)
            {
                return NotFound();
            }

            return View(communicationViewModel);
        }

        // GET: Communications/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Communications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageDate,Subject,Message,Id,Created,communicationAttachment")] tblCommunication tblCommunication)
        {
            var uploadFile = "";
            var originalFileName = "";
            if (Request.Form.Files.Count() > 0)
            {
                IFormFile file = Request.Form.Files[0];
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(Path.GetFileName(file.FileName));
                uploadFile = filename;
                originalFileName = file.FileName;

                string uploadPath = @"wwwroot\Uploads";
                if (!Directory.Exists(Path.Combine(uploadPath)))
                {
                    // If it doesn't exist, create the directory
                    Directory.CreateDirectory(Path.Combine(uploadPath));
                }
                var filePath = uploadPath + "/" + filename;

                await file.CopyToAsync(new FileStream(filePath, FileMode.Create));

                tblCommunication.communicationAttachment = uploadFile;
                tblCommunication.fileName = originalFileName;
            }
            tblCommunication.Created = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("South Africa Standard Time"));
            tblCommunication.Guid = Guid.NewGuid().ToString();
            if (ModelState.IsValid)
            {
                _context.Add(tblCommunication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblCommunication);
        }

        // GET: Communications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCommunication = await _context.tblCommunications.FindAsync(id);
            if (tblCommunication == null)
            {
                return NotFound();
            }
            return View(tblCommunication);
        }

        // POST: Communications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MessageDate,Subject,Message,Id,Created,communicationAttachment,Guid")] tblCommunication tblCommunication)
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

                tblCommunication.communicationAttachment = uploadFile;
            }

            if (id != tblCommunication.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCommunication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblCommunicationExists(tblCommunication.Id))
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
            return View(tblCommunication);
        }

        // GET: Communications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblCommunication = await _context.tblCommunications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblCommunication == null)
            {
                return NotFound();
            }

            return View(tblCommunication);
        }

        // POST: Communications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblCommunication = await _context.tblCommunications.FindAsync(id);
            _context.tblCommunications.Remove(tblCommunication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool tblCommunicationExists(int id)
        {
            return _context.tblCommunications.Any(e => e.Id == id);
        }

        public IActionResult SendEmail(int id)
        {
           // BackgroundJob.Enqueue(() => SendEmailJobAsync(id));

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Send(int id)
        {
           string[] mailinglists = Request.Form["mailinglist"].ToArray();

            BackgroundJob.Enqueue(() => SendEmailJobAsync(id, mailinglists));

            return RedirectToAction(nameof(Index));
        }

        public async Task SendEmailJobAsync(int id, string[] mailinglists = null)
        {
            var tblCommunication = await _context.tblCommunications.FirstOrDefaultAsync(m => m.Id == id);
            
            var tblmailinglist = _context.tblMailingListClients.Include(c => c.tblMailingList)
                .Include(c => c.tblPharmacist)
                .ThenInclude(t => t.ApplicationUser)
                .Where(c => mailinglists.Contains(c.tblMailingListId.ToString()))
                .Select(c => c.tblPharmacist)
                .Distinct();

            foreach (var client in tblmailinglist.ToList())
            {
                await _emailSender.SendEmailWithAttachmentAsync(client.ApplicationUser.Email, tblCommunication.Subject, tblCommunication.Message,tblCommunication.communicationAttachment,tblCommunication.fileName);
                tblCommunicationLogs tblCommunicationLogs = new tblCommunicationLogs();
                tblCommunicationLogs.ClientId = client.ApplicationUserId;
                tblCommunicationLogs.CommunicationId = id;
                tblCommunicationLogs.Created = DateTime.Now;
                tblCommunicationLogs.Receipient = client.ApplicationUser.Email;

                _context.Add(tblCommunicationLogs);
                await _context.SaveChangesAsync();

            }
        }
    }
}
