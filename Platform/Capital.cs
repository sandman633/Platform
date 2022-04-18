using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Platform
{
    public class Capital
    {
        public static async Task Invoke(HttpContext context)
        {
            string country = context.Request.RouteValues["country"] as string;
            string capital = null;
            switch ((country??"").ToLower())
            {
                case "uk":
                    capital = "London";
                    break;
                case "france":
                    capital = "Paris";
                    break;
                case "monaco":
                    var generator = context.RequestServices.GetService<LinkGenerator>();
                    string url = generator.GetPathByRouteValues(context,
                        "population", new { city = country });
                    context.Response.Redirect(url);
                    return;
            } 
            if (capital != null)
            {
                await context.Response.WriteAsync($"Capital of {country} is {capital}");
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
