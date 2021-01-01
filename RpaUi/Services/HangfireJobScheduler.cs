using Hangfire;
using System;
using RpaUi.Jobs;

namespace RpaUi.Services
{
    public class HangfireJobScheduler
    {
        public static void ScheduleRecurringJobs()
        {
            RecurringJob.RemoveIfExists(nameof(MeetingStatusJob));
            RecurringJob.AddOrUpdate<MeetingStatusJob>("Meeting Status",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily, TimeZoneInfo.Local);

            RecurringJob.RemoveIfExists(nameof(BirthdayMessageJob));
            RecurringJob.AddOrUpdate<BirthdayMessageJob>("Birthday Message",
                job => job.Run(JobCancellationToken.Null),
                Cron.Daily, TimeZoneInfo.Local);
        }
    }
}
