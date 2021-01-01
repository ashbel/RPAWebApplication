using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpaUi.Interfaces
{
    public interface IBackgroundJobs
    {
        public Task Run(IJobCancellationToken token);
        public Task RunAtTimeOf(DateTime runDate);
    }
}
