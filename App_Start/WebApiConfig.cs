using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace XiyouNews
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "NewsList",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "NewsList", action = "Index",id=RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "NewsDetail",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { controller = "NewsDetail", action = "Index", id = RouteParameter.Optional }
            );
        }
    }
}
