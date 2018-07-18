using QiBuBlog.Service;
using QiBuBlog.Util;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Controllers
{
    public class ArticleController : BaseController
    {
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }
    }
}
