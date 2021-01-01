using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RpaData.DataContext;
using RpaUi.Interfaces;

namespace RpaUi.Jobs
{
    public class BirthdayMessageJob : IBackgroundJobs
    {
        private readonly ILogger<BirthdayMessageJob> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMyEmailSender _email;

        public BirthdayMessageJob(ILogger<BirthdayMessageJob> logger, ApplicationDbContext context, IMyEmailSender email)
        {
            _context = context;
            _logger = logger;
            _email = email;
        }
        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.Now);
        }

        public async Task RunAtTimeOf(DateTime runDate)
        {
            _logger.LogInformation("My Job Starts ...");
            var dateDay = DateTime.Now.Day;
            var dateMonth = DateTime.Now.Month;
            var members = _context.tblPharmacists
                .Include(t => t.ApplicationUser)
                .Where(c => c.ApplicationUser.DateOfBirth.Day == dateDay && c.ApplicationUser.DateOfBirth.Month == dateMonth );
            foreach (var m in members.ToList())
            {
                await _email.SendEmailAsync(m.ApplicationUser.Email, "Happy Birthday !!",
                    $"Dear {m.ApplicationUser.FullName} <br><br> On behalf of the RPA family we would like to wish you a Happy Birthday.");
            }
            _logger.LogInformation("My Job Completed");
        }
    }
}
