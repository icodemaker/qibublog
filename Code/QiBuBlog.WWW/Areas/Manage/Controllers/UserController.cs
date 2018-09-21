using QiBuBlog.Entity;
using QiBuBlog.Service;
using QiBuBlog.WWW.Controllers;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Manage/User/

        public ActionResult Index(User queryParams, int currentPage = 1, int pageSize = 10)
        {
            var data = new UserService().GetPageList(queryParams, currentPage, pageSize);
            return View(data);
        }
    }
}
