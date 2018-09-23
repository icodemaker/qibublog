using QiBuBlog.Entity;
using QiBuBlog.Service;
using QiBuBlog.Util;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(int currentPage = 1, int pageSize = 10)
        {
            ViewBag.CategoryList = new CategoryService().GetList();
            var data = new ArticleService().GetPageList(null, currentPage, pageSize);
            return View(data);
        }
    }
}
