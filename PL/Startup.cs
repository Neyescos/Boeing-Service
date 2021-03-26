using BLL.Interfaces;
using BLL.MapperProfile;
using BLL.Services;
using DAL;
using DAL.Context;
using DAL.Interfaces;
using DAL.Repositories;
using DAL.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PL.Interfaces;
using PL.MapperProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PL
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/user/Login");
                    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/user/Login");
                });

            services.AddControllersWithViews();
            //DAL
            services.AddTransient<BoengServiceWebSiteContext>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPlaneModelRepository, PlaneModelRepository>();
            services.AddTransient<IPlanePartRepository, PlanePartRepository>();
            //BLL
            services.AddSingleton<IMapperProfile, DtoProfile>();
            services.AddTransient<IEncodingService, PasswordEncodingService>();
            services.AddTransient<IPlaneModelService, PlaneModelService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPlanePartService, PlanePartService>();
            //PL
            services.AddSingleton<IViewProfile, ViewModelProfile>();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
