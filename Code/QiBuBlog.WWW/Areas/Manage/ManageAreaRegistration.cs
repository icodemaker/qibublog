using System.Web.Mvc;

namespace QiBuBlog.WWW.Areas.Manage
{
    public class ManageAreaRegistration: AreaRegistration
    {
        public override string AreaName => "Manage";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.LowercaseUrls = true;

            context.MapRoute(
                    name: "QiBuBlog.WWW.Areas.Manage",
                    url: "Manage/{controller}/{action}/{id}",
                    defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                    namespaces: new[] { "QiBuBlog.WWW.Areas.Manage.Controllers" }
                );
        }
    }
}