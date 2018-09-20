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

        public ActionResult Index(string username, int? groupid, string nickname, int page = 1)
        {
            var urlParams = new Dictionary<string, string>
            {
                {"username", username},
                {"groupid", groupid.GetValueOrDefault().ToString()},
                {"nickname", nickname}
            };

            var userList = new UserService().GetPageList(page, 5);

            ViewBag.Pager = userList.TotalRecord < 1 ? string.Empty :(new HtmlPager(Request.Path.ToLower(), urlParams)).GenerateCode(userList.TotalRecord/5, page);

            return View(userList);
        }

    }
}
