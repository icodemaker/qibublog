using System.Web.Mvc;
using System.Web.Routing;

namespace QiBuBlog.WWW
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;//路径小写

            routes.MapRoute(
                name: "QiBuBlog",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "QiBuBlog.WWW.Controllers" }
            );
        }
    }
}