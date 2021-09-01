using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OauthClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(configureOptions: config =>
            {
                // to ensure we are authenticated (through cookie)
                config.DefaultAuthenticateScheme = "ClientCookie";

                config.DefaultSignInScheme = "ClientCookie";

                // to check if we are allowed to do somthing
                config.DefaultChallengeScheme = "OurServer";


            })
                    .AddCookie("ClientCookie")
                    .AddOAuth("OurServer", configureOptions: config =>
                    {
                        config.ClientId= "client_id";
                        config.ClientSecret= "client_secret";
                        config.CallbackPath = "/oauth/callback";
                        config.AuthorizationEndpoint = "https://localhost:5001/Oauth/Authorization";
                        config.TokenEndpoint = "https://localhost:5001/Oauth/Token";
                        config.Events = new OAuthEvents()
                        {
                            OnRemoteFailure = (context) =>
                            {
                                Console.WriteLine(context.Failure);
                                context.Response.Redirect("/");
                                context.HandleResponse();
                                return Task.CompletedTask;
                            }
                        };
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
