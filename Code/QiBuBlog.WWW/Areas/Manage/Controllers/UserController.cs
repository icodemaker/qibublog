using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QiBuBlog.Service;
using QiBuBlog.Util;
using QiBuBlog.WWW.Controllers;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Manage/User/

        public ActionResult Index()
        {
            LogHelper.Debug("用户列表");
            var userList = UserService.Instance.GetPageList();
            return View(userList);
        }

    }
}
