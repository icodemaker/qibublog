using QiBuBlog.Service;
using QiBuBlog.WWW.Controllers;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Manage/User/

        public ActionResult Index()
        {
            var userList = new UserService().GetPageList();
            return View(userList);
        }

    }
}
