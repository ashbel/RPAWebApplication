using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using RpaData.DataContext;
using RpaUi.Interfaces;

namespace RpaUi.Jobs
{
    public class MemberStatusJob : IBackgroundJobs
    {
        private readonly ILogger<MemberStatusJob> _logger;
        private readonly ApplicationDbContext _context;

        public MemberStatusJob(ILogger<MemberStatusJob> logger, ApplicationDbContext context)
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
            //do work
            await _context.SaveChangesAsync();
            _logger.LogInformation("My Job Completed");
        }
    }
}
