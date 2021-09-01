using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
{
    public class Startup
    {
       
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(config => {
                config.DefaultScheme = "cookie";
                config.DefaultChallengeScheme = "oidc";
            })
                    .AddCookie("cookie")
                    .AddOpenIdConnect("oidc", config => {
                        config.Authority = "https://localhost:5001/";
                        config.ClientId = "client_id_mvc";
                        config.ClientSecret = "client_secret_mvc";
                        config.SaveTokens = true;
                        config.ResponseType = "code";
                        // configure cookie claim mapping 
                        config.ClaimActions.MapUniqueJsonKey("RawCliam", "myCustomeCliam");

                        config.GetClaimsFromUserInfoEndpoint = true;
                        // config custom cliam if this claim won't present it will be unauth.
                        // if idtype is set to true in identidty server
                        config.Scope.Add("rc.scope");
                    });
            services.AddControllersWithViews();
        }

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
