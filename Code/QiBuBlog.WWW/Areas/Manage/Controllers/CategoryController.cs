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
    public class CategoryController : BaseController
    {
        //
        // GET: /Manage/Category/

        public ActionResult Index(Category parameters, int currentPage = 1, int pageSize = 10)
        {
            var data = new CategoryService().GetPageList(parameters, currentPage, pageSize);
            return View(data);
        }

    }
}
