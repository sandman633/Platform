using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;

namespace Platform
{
    public class CountryRouteConstraint : IRouteConstraint
    {
        private static string[] countries = { "uk", "france", "monaco" };
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string segment = values[routeKey] as string ?? "";
            return Array.IndexOf(countries, segment.ToLower()) > -1;
        }
    }
}