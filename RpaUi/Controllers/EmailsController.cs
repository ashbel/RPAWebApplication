using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpaData.DataContext;
using RpaData.Models;

namespace RpaUi.Controllers
{
    [Authorize]
    public class EmailsController : Controller
    {
        private readonly ApplicationDbContext db;

        public EmailsController( ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            tblEmail tblEmail = db.tblEmails.FirstOrDefault();
            if (tblEmail == null)
            {
                tblEmail = new tblEmail();
                return View(tblEmail);
            }
            return View(tblEmail);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Id,email_smtp,email_address,email_port,email_username,email_password,email_ssl")] tblEmail tblEmails)
        {
            //if (ModelState.IsValid)
            //{
            //    int? id = tblEmails.Id;
            //    if (id == null)
            //    {
            //        return NotFound();
            //    }
            //    tblEmail tblemail = db.tblEmails.FirstOrDefault();
            //    if (tblemail == null)
            //    {
            //        db.tblEmails.Add(tblEmails);
            //        db.SaveChanges();
            //    }
            //    else
            //    {
            //        tblemail.email_address = tblEmails.email_address;
            //        tblemail.email_password = tblEmails.email_password;
            //        tblemail.email_port = tblEmails.email_port;
            //        tblemail.email_smtp = tblEmails.email_smtp;
            //        tblemail.email_ssl = tblEmails.email_ssl;
            //        tblemail.email_username = tblEmails.email_username;
            //        db.SaveChanges();
            //    }
 
            //    return View(tblEmails);
            //}

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(tblEmails);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!tblEmailsExists(tblEmails.Id))
                    {
                        db.Add(tblEmails);
                        await db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tblEmails);
        }

        private bool tblEmailsExists(int id)
        {
            return db.tblEmails.Any(e => e.Id == id);
        }
    }
}