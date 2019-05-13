using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TamsPizzeriaWebApp.Data;
using TamsPizzeriaWebApp.Models;
using TamsPizzeriaWebApp.Services;
using TamsPizzeriaWebApp.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using TamsPizzeriaWebApp.Authorization.Handlers;

namespace TamsPizzeriaWebApp
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
            services.AddMvc();
           
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add authorization policy
            services.AddAuthorization(options =>
            {
            options.AddPolicy(
                "IsEmployee",
                policyBuilder => policyBuilder.AddRequirements(
                    new EmployeeRequirement()
                    ));
            });
            
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });


            // Add the handlers for handling the custom policy requirements
            services.AddScoped<IAuthorizationHandler, IsEmployeeHandler>();

            // Add application services.
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<IPizzeriaMenu, PizzeriaMenu>();
            services.AddScoped<IOrder, Ordering>();
            services.AddScoped<IOrderHistory, OrderHistory>();

            //// Add the following line of code for creating DI for UserClaims.
            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MyUserClaimsPrincipalFactory>();

            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                // Experimentation with routing.
                routes.MapRoute(
                    name: "employeeService",
                    template: "{controller}/{action}/{service}",
                    defaults: new {
                        controller = "Employee",
                        action = "Service",
                        service = "Portal"
                });
            });
        }
    }
}
