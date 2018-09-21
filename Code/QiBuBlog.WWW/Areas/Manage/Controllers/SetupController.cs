using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QiBuBlog.Entity;
using QiBuBlog.Service;
using QiBuBlog.WWW.Controllers;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class SetupController : BaseController
    {
        //
        // GET: /Manage/Setup/

        public ActionResult Index()
        {
            return View(new SetupService().GetSetup());
        }

        [HttpPost]
        public ActionResult Index(Setup parameters)
        {
            new SetupService().UpdateSetup(parameters);
            return View(parameters);
        }
    }
}
