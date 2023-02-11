using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineMagazin.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using OnlineMagazin.Areas.Identity.Data;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using OnlineMagazin.Models;
using OnlineMagazin.Service;

namespace OnlineMagazin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<CookieAuthenticationEvents>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddControllersWithViews(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));

            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => { x.LoginPath = "/Identity/Account/Login"; });
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.Configure<IdentityOptions>(options => { options.SignIn.RequireConfirmedEmail= true; });
            services.AddIdentity<OnlineMagazinUser, IdentityRole>()
                    .AddEntityFrameworkStores<OnlineMagazinContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();
            services.AddControllersWithViews();

            //services.AddDefaultIdentity<OnlineMagazinUser>(options => options.SignIn.RequireConfirmedAccount = false)
            //        .AddRoles<IdentityRole>()
            //        .AddEntityFrameworkStores<OnlineMagazinContext>();
                //var connection = "server=DESKTOP-2IIQV65\\SQLEXPRESS; database=OnlineMagazin; integrated security=true";
                services.AddDbContext<OnlineMagazinContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OnlineMagazinString")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/ErrorPage/ErrorName","?code={0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=HomeIndex}/{id?}");
                endpoints.MapRazorPages();
            });

        }
    }
}
