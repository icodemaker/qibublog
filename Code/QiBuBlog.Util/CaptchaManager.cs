using System;
using System.Web;

namespace QiBuBlog.Util
{
    public abstract class CaptchaManager
    {

        private const string SessionPrefix = "Captcha-";

        public static void Write(string formName, string captcha)
        {
            HttpContext.Current.Session[SessionPrefix + formName] = captcha;
        }

        private static void Clear(string formName)
        {
            HttpContext.Current.Session.Remove(SessionPrefix + formName);
        }

        public static bool Check(string formName, string captcha)
        {
            var val = HttpContext.Current.Session[SessionPrefix + formName];
            var result = val != null && !string.IsNullOrEmpty(captcha) && string.Equals(val.ToString(), captcha, StringComparison.CurrentCultureIgnoreCase);

            Clear(formName);

            return result;
        }
    }
}
