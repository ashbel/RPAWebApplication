using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(RpaUi.Areas.Identity.IdentityHostingStartup))]
namespace RpaUi.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}