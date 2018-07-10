using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace QiBuBlog.Util
{
    public sealed class Encrypt
    {
        public static string ToSHA1(string s)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(s, "SHA1");
        }
    }
}
