using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform
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
            services.Configure<RouteOptions>(options =>{
                options.ConstraintMap.Add("countryName",typeof(CountryRouteConstraint));
            });
            services.AddRazorPages();
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

            app.UseStaticFiles();

            app.UseRouting();

            //app.UseMiddleware<Population>();
            //app.UseMiddleware<Capital>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapGet("{first:alpha:length(3)}/{second:bool}/{third?}", async context =>
                {
                    await context.Response.WriteAsync("Request was routed!");
                    foreach (var kvp in context.Request.RouteValues)
                    {
                        await context.Response.WriteAsync($"\r\n {kvp.Key} : {kvp.Value}");
                    }
                });
                endpoints.MapGet("size/{city}", Population.Invoke).WithMetadata(new RouteNameMetadata("population")); ;
                endpoints.MapGet("capital/{country:countryName}", Capital.Invoke);
                endpoints.MapFallback(async context =>
                {
                    await context.Response.WriteAsync($"\r\n Fallback routed");
                });
            });
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Terminal midldeware Reached");
            });
        }
    }
}
