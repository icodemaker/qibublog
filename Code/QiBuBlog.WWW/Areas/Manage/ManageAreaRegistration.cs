using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Areas.Manage
{
    public class ManageAreaRegistration: AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                    name: "QiBuBlog.WWW.Areas.Manage",
                    url: "Manage/{controller}/{action}/{id}",
                    defaults: new { controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                    namespaces: new[] { "QiBuBlog.WWW.Areas.Manage.Controllers" }
                );
        }
    }
}