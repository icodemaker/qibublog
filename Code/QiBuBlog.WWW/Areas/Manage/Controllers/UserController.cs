using System;
using System.Collections.Generic;
using QiBuBlog.Service;
using QiBuBlog.WWW.Controllers;
using System.Web.Mvc;
using QiBuBlog.Util;

namespace QiBuBlog.WWW.Areas.Manage.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /Manage/User/

        public ActionResult Index(string username, int? groupid, string nickname, int? page)
        {
            var urlParams = new Dictionary<string, string>
            {
                {"username", username},
                {"groupid", groupid.GetValueOrDefault().ToString()},
                {"nickname", nickname}
            };

            var userList = new UserService().GetPageList();

            ViewBag.Pager = userList.TotalRecord < 1 ? string.Empty :(new HtmlPager(Request.Path, urlParams)).GenerateCode(20, 1);

            return View(userList);
        }

    }
}
