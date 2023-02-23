using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineMagazin.Areas.Identity.Data;
using OnlineMagazin.Data;

[assembly: HostingStartup(typeof(OnlineMagazin.Areas.Identity.IdentityHostingStartup))]
namespace OnlineMagazin.Areas.Identity
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
