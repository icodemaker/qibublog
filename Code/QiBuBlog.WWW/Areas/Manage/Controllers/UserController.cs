using QiBuBlog.Service;
using QiBuBlog.WWW.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Manage/User/

        public ActionResult Index(string username, string nickname, int currentPage = 1, int pageSize = 10)
        {
            var urlParams = new Dictionary<string, string>
            {
                {"username", username},
                {"nickname", nickname}
            };

            var data = new UserService().GetPageList(urlParams, currentPage, pageSize);

            return View(data);
        }
    }
}
