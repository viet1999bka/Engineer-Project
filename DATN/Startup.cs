using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using DATN.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace DATN
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
            var stringConnectdb = Configuration.GetConnectionString("dbProjectEnd");
            services.AddDbContext<EndProjectContext>(options => options.UseSqlServer(stringConnectdb));
            services.AddSession();
            services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.All }));
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //            .AddCookie(
            //            options =>
            //            {
            //                options.LoginPath = "/dang-nhap.html";
            //                options.AccessDeniedPath = "/";
            //            });
            services
                    .AddAuthentication(o =>
                    {
                        o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie("frontend", o =>
                    {
                        o.LoginPath = new PathString("/dang-nhap.html");
                        o.AccessDeniedPath = "/";
                    })
                    .AddCookie("backend", o =>
                    {
                        o.LoginPath = new PathString("/admin/dang-nhap-admin.html");
                        o.AccessDeniedPath = "/admin/";
                    });
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseNotyf();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
