using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using RpaData.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using RpaUi.Interfaces;

namespace RpaUi.Services
{
    public class EmailSender : IMyEmailSender
    {
        private readonly ApplicationDbContext _context;
        private string uploadPath = @"wwwroot\Uploads";

        public EmailSender(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendEmailWithAttachmentAsync(string email, string subject, string htmlMessage, string attachment = "", string filename = "")
        {
            //BackgroundJob.Enqueue(() => SendEmail(email, subject,htmlMessage));
            await SendEmail(email, subject, htmlMessage, attachment, filename);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //BackgroundJob.Enqueue(() => SendEmail(email, subject,htmlMessage));
            await SendEmail(email, subject, htmlMessage);
        }

        public async Task SendEmail(string email, string subject, string htmlMessage, string htmlAttachment = "", string filename="")
        {
            var email_config = _context.tblEmails.FirstOrDefault();

            var emailMessage = new MailMessage();


            emailMessage.From = new MailAddress(email_config.email_address, "Retail Pharmacists Association");

            emailMessage.To.Add(email);
            emailMessage.Subject = subject;
            emailMessage.Body = htmlMessage + "<p> Regards </p> <p><b> Retail Pharmacists Association </b></p>";
            emailMessage.IsBodyHtml = true;

            if (!String.IsNullOrEmpty(htmlAttachment))
            {
                var attachment = new Attachment(uploadPath + "/" + htmlAttachment);

                if (!string.IsNullOrEmpty(filename))
                {
                    attachment.Name = filename;
                }
                
                emailMessage.Attachments.Add(attachment);
            }

            using (var client = new SmtpClient())
            {
                client.Host = email_config.email_smtp;
                client.Port = Convert.ToInt32(email_config.email_port);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(email_config.email_username, email_config.email_password);
                client.Timeout = 20000;
                await client.SendMailAsync(emailMessage).ConfigureAwait(false);
                //await client.(true).ConfigureAwait(false);
            }
        }

    }
}
