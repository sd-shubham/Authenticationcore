using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SimpleJWt.Controllers;

namespace SimpleJWt
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("JwtTokenAuth")
                    .AddJwtBearer("JwtTokenAuth", config =>
                    {
                        var secretBytes = Encoding.UTF8.GetBytes(Constant.SecurityKey);
                        var key = new SymmetricSecurityKey(secretBytes);
                        // if you want to pass tokem in url
                        config.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (context.Request.Query.ContainsKey("access_token"))
                                {
                                    context.Token = context.Request.Query["access_token"];
                                }
                                return Task.CompletedTask;
                            }
                        };
                        //
                        config.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = key,
                            ValidIssuer = Constant.Issuers,
                            ValidAudience = Constant.Audiance
                        };
                    });
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
