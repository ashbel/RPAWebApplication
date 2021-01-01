using System;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using RpaData.DataContext;
using RpaUi.Interfaces;

namespace RpaUi.Jobs
{
    public class MeetingStatusJob : IBackgroundJobs
    {
        private readonly ILogger<MeetingStatusJob> _logger;
        private readonly ApplicationDbContext _context;

        public MeetingStatusJob(ILogger<MeetingStatusJob> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.Now);
        }

        public async Task RunAtTimeOf(DateTime runDate)
        {
           _logger.LogInformation("My Job Starts ...");
           var dateNow = DateTime.Now;
           var meetings = _context.tblEvents.Where(c => c.EventEndDate <= dateNow && !c.EventComplete);
           foreach (var m in meetings.ToList())
           {
               m.EventComplete = true;
               _context.Update(m);
               await _context.SaveChangesAsync();
           }
           _logger.LogInformation("My Job Completed");
        }
    }
}
