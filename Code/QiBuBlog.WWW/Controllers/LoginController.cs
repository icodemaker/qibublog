using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            Captcha captcha = new Captcha(4);
            CaptchaManager.Write("login", captcha.Value);
            Response.Cookies.Add(new HttpCookie("TMS_Captcha", captcha.Value));
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
    }
}
