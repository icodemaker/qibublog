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

        public ActionResult Index(string id, int? page)
        {
            ViewBag.CurrentPosition = id;
            var categoryId = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                var cId = new MenuService().GetMenuCategoryId(id);
                if (!string.IsNullOrWhiteSpace(cId))
                {
                    categoryId = cId;
                }
            }
            var articleData = new ArticleService().GetPageList(categoryId, page ?? 1, true);
            return View(articleData);
        }
    }
}
