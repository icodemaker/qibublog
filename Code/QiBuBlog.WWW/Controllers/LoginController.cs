using QiBuBlog.Service;
using QiBuBlog.Util;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace QiBuBlog.WWW.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCaptcha()
        {
            var captcha = new Captcha(4);
            CaptchaManager.Write("login", captcha.Value);
            Response.Cookies.Add(new HttpCookie("QiBu_Captcha", captcha.Value));
            Image image = captcha.CreateImage(
                Color.FromArgb(
                    Convert.ToInt32(255),
                    Convert.ToInt32(255),
                    Convert.ToInt32(255)
                ), 170, 15, 4, 5
            );
            var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            stream.Close();
            return File(stream.GetBuffer(), @"image/png");
        }

        public JsonResult Authority(string tenantCode, string loginName, string password)
        {
            
            var user = UserService.Instance.GetUserForLogin(loginName, password);
            if (user != null)
            {
                
            }
            else
            {
                
            }
            return Json(null);
        }

        [SelfOnly]
        public ActionResult Logout()
        {
            return View("Index");
        }
    }
}
