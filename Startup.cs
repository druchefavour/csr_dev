using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.IO;
using System.Net.Http;
using System.Net;

namespace Ibis_CSR_Tool
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
            var settings = new Settings();
            Configuration.Bind(settings);

            services.AddSingleton(settings);

          /*  services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //  sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
             .AddCookie();*/
    
             //.AddOpenIdConnect(options =>
             //{
             //    options.ClientId = Configuration["okta:ClientId"];
             //    options.ClientSecret = Configuration["okta:ClientSecret"];
             //    options.Authority = Configuration["okta:Issuer"];
             //    options.CallbackPath = "/authorization-code/callback";
             //    options.ResponseType = "code";
             //    options.SaveTokens = true;
             //    options.UseTokenLifetime = false;
             //    options.GetClaimsFromUserInfoEndpoint = true;
             //    options.Scope.Add("openid");
             //    options.Scope.Add("profile");
             //    options.TokenValidationParameters = new TokenValidationParameters
             //    {
             //        NameClaimType = "name"
             //    };
             //});

            services.AddControllersWithViews();
            services.AddHttpClient();

            // In production, the Angular files will be served from this directory
           services.AddSpaStaticFiles(configuration =>
             {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddMvc(options =>
            {
            });
              services.AddHttpClient("SampleClient", httpClient =>
            {
                httpClient.BaseAddress = new
                Uri("https://webappsdev.iltest.illinois.gov/DES/CSRTool");
            })
            .ConfigurePrimaryHttpMessageHandler((handler) =>
                new HttpClientHandler
                {
                    Proxy = new WebProxy("http://webgateway.illinois.gov:9090", true)
                });

            services.AddControllersWithViews();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    });

            });
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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
                app.UseExceptionHandler("/Error");
            }

            app.Use(async (context, next) =>
            {
                await next();
                if(context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
            {
                context.Request.Path="/index.html";
                await next();
            }
        });

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

        //    app.UseAuthentication();

            app.UseCors("CorsPolicy");

            app.UseDefaultFiles();

            app.UseStaticFiles();

           // app.UseAuthorization();
            app.UseHttpsRedirection(); 
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            //app.UseMvc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

           /* app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });*/
        }
    }
}
 