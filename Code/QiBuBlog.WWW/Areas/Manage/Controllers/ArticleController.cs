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
    public class ArticleController : BaseController
    {
        //
        // GET: /Manage/Article/

        public ActionResult Index(ArticleListView parameters, int currentPage = 1, int pageSize = 10)
        {
            ViewBag.CategoryList = new CategoryService().GetList();
            var data = new ArticleService().GetPageList(parameters, currentPage, pageSize);
            return View(data);
        }
    }
}
