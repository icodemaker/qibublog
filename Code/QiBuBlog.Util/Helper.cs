using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace QiBuBlog.Util
{
    public class Helper
    {
        #region 其他公共部分
        public static string GetIpAddress()
        {
            string userIp;
            try
            {
                var resqust = HttpContext.Current.Request;

                userIp = !string.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrWhiteSpace(userIp))
                {
                    userIp = HttpContext.Current.Request.UserHostAddress;
                }
            }
            catch (Exception)
            {
                userIp = "127.0.0.1";
            }
            return userIp;
        }

        public static string HttpGet(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            WebResponse response = null;
            string responseStr = null;
            try
            {
                response = request.GetResponse();
                {
                    var reader = new StreamReader(response?.GetResponseStream(), encoding: Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                response = null;
            }
            return responseStr;
        }

        public static string HttpPost(string url, string param)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            StreamWriter requestStream = null;
            WebResponse response = null;
            string responseStr = null;
            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();
                response = request.GetResponse();
                {
                    var reader = new StreamReader(response.GetResponseStream(), encoding: Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                response = null;
            }
            return responseStr;
        }
        #endregion
    }
}
