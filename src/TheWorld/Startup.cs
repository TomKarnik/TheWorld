using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TheWorld.Services;
using Microsoft.Extensions.Configuration;
using TheWorld.Models;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using TheWorld.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TheWorld
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {   
            // Via dependence injection capture hosting environment
            _env = env;

            // Setup JSON configuration file
            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();
                
            _config = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Register applicatioiin configuration file (JSON)
            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development") || _env.IsEnvironment("Testing"))
            {
                // Register debug mail service
                services.AddScoped<IMailService, DebugMailService>();
            }
            else
            {
                // Actual mail service!
            }

            // Register our database context
            services.AddDbContext<WorldContext>();

            // Register World Repository
            services.AddScoped<IWorldRespository, WorldRespository>();

            // Register Bing Maps GEO service
            services.AddTransient<GeoCoordsService>();

            // Register our database seed class
            services.AddTransient<WorldContextSeedData>();

            // Add logging
            services.AddLogging();

            // Register MVC
            services.AddMvc(config => 
            {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            })
            .AddJsonOptions(configure => 
            {
                configure.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            // Add and configure Identy functionality
            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        await Task.Yield();
                    }
                };
            })
            .AddEntityFrameworkStores<WorldContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,
            WorldContextSeedData seeder, ILoggerFactory factory)
        {
            // Initialize AutoMapper for TripsController
            Mapper.Initialize(config =>
            {
                // This configuration also works for collection of Trips and TripViewModels as well!
                config.CreateMap<TripViewModel, Trip>().ReverseMap(); // <- ReverseMap allows both map directions!
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });

            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Add debug logging
                factory.AddDebug(LogLevel.Information);
            }
            else
            {
                // Add debug logging
                factory.AddDebug(LogLevel.Error);
            }

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("<html><body><h3>Hello World!</h3></body></html>");
            //});

            // Using default file mapping and using static files
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            // Turn on Identity
            app.UseIdentity();

            // Now using MVC
            app.UseMvc(config => {
                config.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new {controller = "App", action = "Index" });

            });

            // Seed the database data if needed
            seeder.EnsureSeedData().Wait();
        }
    }
}
