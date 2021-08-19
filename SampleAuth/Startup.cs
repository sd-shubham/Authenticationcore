using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SampleAuth.AuthRequirement;
using SampleAuth.CustomePolicyProvider;

namespace SampleAuth
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CookieAuth")
                    .AddCookie("CookieAuth", config =>
                    {
                        config.Cookie.Name = "Simple.auth.cookie";
                        config.LoginPath = "/Home/Index";
                    });
            services.AddAuthorization(config =>
            {
                // var defaultAuthBuilder = new AuthorizationPolicyBuilder();
                // var defaultAuthPolicy = defaultAuthBuilder
                //                         .RequireAuthenticatedUser()
                //                         .RequireClaim(ClaimTypes.DateOfBirth)
                //                         .Build();
                config.AddPolicy("Claim.DOB", policy =>
                {
                    // policy.RequireClaim(ClaimTypes.DateOfBirth);
                    // policy.AddRequirements(new CustomRequireCliam(ClaimTypes.DateOfBirth));


                    /** Setting up the cliams that every req must hv**/ 
                    policy.RequireCustomeClaim(ClaimTypes.DateOfBirth);
                });
            });
             /* validating the claims line 47*/
            services.AddScoped<IAuthorizationHandler, CustomRequireCliamHandler>();
            services.AddSingleton<IAuthorizationPolicyProvider, CustomeAuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, SecurityHandler>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
