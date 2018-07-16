using System.Text.RegularExpressions;

namespace QiBuBlog.Util
{
    public static class Validator
    {
        private static readonly Regex IP = new Regex(@"\d+(?:\.\d+){3}", RegexOptions.Compiled);

        private static readonly Regex Email = new Regex(@"^([.a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]+)*)\.[a-zA-Z]+$", RegexOptions.Compiled);

        private static readonly Regex BeginWithHttp = new Regex(@"^https?://", RegexOptions.IgnoreCase);

        public static bool IsIP(string val)
        {
            if (!IP.IsMatch(val))
            {
                return false;
            }

            var sections = val.Split(new[] { '.' });
            foreach (var i in sections)
            {
                byte temp;
                if (!byte.TryParse(i, out temp))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsEmail(string val)
        {
            return Email.IsMatch(val);
        }

        public static bool IsQQ(string val)
        {
            return Regex.IsMatch(val, @"^\d{5,}$");
        }

        public static bool IsNumbers(string val)
        {
            return Regex.IsMatch(val, @"\d+(?:,\d+)*");
        }

        public static string FixUrl(string url)
        {
            return BeginWithHttp.IsMatch(url) ? url : "http://" + url;
        }
    }
}
