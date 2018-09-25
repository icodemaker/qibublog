using System.Web.Mvc;
using System.Web.Routing;

namespace QiBuBlog.WWW
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                "QiBuBlog",
                "{controller}/{action}/{id}",
                new { controller = "Article", action = "Index", id = UrlParameter.Optional },
                new[] { "QiBuBlog.WWW.Controllers" }
            );
        }
    }
}