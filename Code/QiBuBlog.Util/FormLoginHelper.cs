using System.Web;
using System.Web.Security;

namespace QiBuBlog.Util
{
    public static class FormLoginHelper<T> where T : class
    {

        public static void Set(T user, bool createPersistentCookie = false)
        {
            var strUser = JSONHelper.Encode<T>(user);
            FormsAuthentication.SetAuthCookie(strUser, createPersistentCookie);
        }

        public static T Get()
        {
            var strUser = HttpContext.Current.User.Identity.Name;
            return JSONHelper.Decode<T>(strUser);
        }

        public static void Logout(string redirectUrl = "/login")
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Response.Redirect(redirectUrl);
        }
    }
}
