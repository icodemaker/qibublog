using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace QiBuBlog.Util
{
    public class Helper
    {
        #region 其他公共部分
        public static string GetIPAddress()
        {
            string userIP;
            try
            {
                var resqust = HttpContext.Current.Request;
                if (resqust == null)
                {
                    throw new Exception("resqust is null!");
                }
                // 如果使用代理，获取真实IP
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    userIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                }
                else
                {
                    userIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (string.IsNullOrWhiteSpace(userIP))
                {
                    userIP = HttpContext.Current.Request.UserHostAddress;
                }
            }
            catch (Exception)
            {
                userIP = "127.0.0.1";
            }
            return userIP;
        }

        public static string HttpGet(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            WebResponse response = null;
            string responseStr = null;
            try
            {
                response = request.GetResponse();
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
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
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
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
                if (response != null)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
