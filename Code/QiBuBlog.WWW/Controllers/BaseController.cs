using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Controllers
{
    [SelfOnly]
    public class BaseController : Controller
    {
        public JsonResult LocalUploadFiles()
        {
            return Json(null);
        }

        public JsonResult AliUploadFiles(HttpPostedFileBase fileData)
        {
            return Json(null);
        }
    }
}
