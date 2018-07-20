using QiBuBlog.Service;
using QiBuBlog.Util;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index(string categoryId, int? page)
        {
            ViewBag.CurrentPosition = categoryId;

            var articleData = ArticleService.Instance.GetPageList(categoryId, page ?? 1);

            //if (articleData.PageCount <= 1) return View(articleData);
            //var pagerHelper = new HtmlPager(Url.Action("Index", new { id = categoryId }), null) { HrefPattern = "{0}/{2}" };
            //ViewBag.ArticlePager = pagerHelper.GenerateCode(articleData.PageCount, articleData.CurrentPage);
            ViewBag.ArticleData = articleData;

            return View(articleData);
        }
    }
}
