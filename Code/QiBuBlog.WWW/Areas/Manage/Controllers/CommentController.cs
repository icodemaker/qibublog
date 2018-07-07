using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class CommentController : BaseController
    {
        //
        // GET: /Manage/Comment/

        public ActionResult Index()
        {
            return View();
        }

    }
}
