using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Platform
{
    public class Population
    {
        public static async Task Invoke(HttpContext context)
        {

            string city = context.Request.RouteValues["city"] as string;
            int? pop = null;

            switch (city.ToLower())
            {
                case "london":
                    pop = 8_136_000;
                    break;
                case "paris":
                    pop = 2_141_000;
                    break;
                case "monaco":
                    pop = 39_000;
                    break;
                default:
                    break;
            }

            if (pop.HasValue)
            {
                await context.Response.WriteAsync($"City: {city}, Population: {pop}");
                return;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
            }
        }
    }
}
