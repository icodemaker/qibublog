using QiBuBlog.Entity;
using QiBuBlog.Service;
using QiBuBlog.WWW.Controllers;
using System.Web.Mvc;

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

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var model = new GetArticleById_Result();
            ViewBag.CategoryList = new CategoryService().GetList();
            if (!string.IsNullOrWhiteSpace(id))
            {
                model = new ArticleService().GetArticleById(id);
            }
            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article model)
        {
            ViewBag.CategoryList = new CategoryService().GetList();
            new ArticleService().CreateOrUpdate(model);
            return View(model);
        }

        public ActionResult Delete(string id, int currentPage = 1, int pageSize = 10)
        {
            var article = new ArticleService();
            article.Delete(id);
            ViewBag.CategoryList = new CategoryService().GetList();
            var data = article.GetPageList(null, currentPage, pageSize);
            return View("Index", data);
        }
    }
}
