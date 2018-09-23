using QiBuBlog.Service;
using QiBuBlog.Util;
using System.Web.Mvc;
using QiBuBlog.Entity;

namespace QiBuBlog.WWW.Controllers
{
    public class ArticleController : BaseController
    {
        //
        // GET: /Article/

        public ActionResult Index(int currentPage = 1, int pageSize = 10)
        {
            ViewBag.CategoryList = new CategoryService().GetList();
            var data = new ArticleService().GetPageList(null, currentPage, pageSize);
            return View(data);
        }

        public ActionResult Detail(string id)
        {
            var model = new ArticleService().GetArticleById(id);
            return View(model);
        }
    }
}
