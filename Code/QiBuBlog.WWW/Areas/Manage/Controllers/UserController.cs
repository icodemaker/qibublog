using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QiBuBlog.WWW.Controllers;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Manage/User/

        public ActionResult Index()
        {
            return View();
        }

    }
}
