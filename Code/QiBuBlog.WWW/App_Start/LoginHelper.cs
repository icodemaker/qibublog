using System;
using System.Web;
using Newtonsoft.Json;
using QiBuBlog.Entity;
using QiBuBlog.Util;

namespace QiBuBlog.WWW
{
    public class LoginHelper
    {
        private static readonly string AuthCookieName = "BlogAuth";
        private const string SiteDomain = "qibu.net.cn";

        private static readonly string desKey = "9$</Zu!j";

        public static void SetAuthCookie(string userInfo, bool createPersistentCookie = false)
        {
            var desValue = MD5AndXOREncrypt.DesEncrypt(userInfo, desKey);
            var authCookie = new HttpCookie(AuthCookieName, desValue)
            {
                Domain = SiteDomain,
                Expires = DateTime.Now.AddDays(1)
            };
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }

        public static User GetUserInfo()
        {
            var requestCookie = HttpContext.Current.Request.Cookies[AuthCookieName];
            if (requestCookie == null) return null;
            var cookieValue = requestCookie.Value;
            if (!string.IsNullOrWhiteSpace(cookieValue))
            {
                var userInfo = MD5AndXOREncrypt.DesDecrypt(cookieValue, desKey);
                return JsonConvert.DeserializeObject<User>(userInfo);
            }
            else
            {
                return null;
            }
        }

        public static void SignOut()
        {
            var authCookie = HttpContext.Current.Response.Cookies[AuthCookieName];
            if (authCookie == null) return;
            authCookie.Expires = DateTime.Now.AddDays(-1);
            authCookie.Domain = SiteDomain;
        }

        private static string MakeServerKey()
        {
            var chars = new string[]{
                "a","b","c","d","e","f","g","h",
                "i","j","k","l","m","n","o","p",
                "q","r","s","t","u","v","w","x",
                "y","z","0","1","2","3","4","5",
                "6","7","8","9","A","B","C","D",
                "E","F","G","H","I","J","K","L",
                "M","N","O","P","Q","R","S","T",
                "U","V","W","X","Y","Z"
            };

            var ra = new Random(unchecked((int)DateTime.Now.Ticks));
            const int num = 8;
            var arrNum = new string[num];
            for (var i = 0; i <= num - 1; i++)
            {
                var tmp = ra.Next(0, 61);
                arrNum[i] = chars[tmp];
            }
            return "@" + string.Concat(arrNum);
        }
    }
}