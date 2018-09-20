using System;
using System.Collections.Generic;
using System.Text;

namespace QiBuBlog.Util
{
    public class HtmlPager
    {
        public const string PageLinkTpl = "<li class=\"{0}\"><a href=\"{2}\">{1}</a></li>";

        public const string PageUpDownTpl = "<span class=\"{0}\"><a href=\"{2}\">{1}</a></span>";

        public const string PageTextTpl = "<li class=\"{0}\"><span>{1}</span></li>";

        public string HrefPattern { get; set; }

        public string BaseUrl { get; set; }

        public Dictionary<string, string> Params { get; set; }

        public HtmlPager(string baseUrl)
        {
            this.BaseUrl = baseUrl;
        }

        public HtmlPager(string baseUrl, Dictionary<string, string> urlParams)
        {
            this.BaseUrl = baseUrl;
            this.Params = urlParams;
        }

        public string GenerateCode(int pageCount, int currentPage)
        {
            if (pageCount <= 1)
            {
                return string.Empty;
            }

            var result = new StringBuilder();
            if (Params != null)
            {
                foreach (var p in Params)
                {
                    result.Append(p.Key);
                    result.Append("=");
                    result.Append(p.Value);
                    result.Append("&amp;");
                }
            }
            result.Append("page=");

            var link = string.Format(string.IsNullOrEmpty(HrefPattern) ? "{0}?{1}{2}" : HrefPattern, BaseUrl, result, "{0}");
            result.Clear();

            result.Append("<div class=\"pager\">");

            if (currentPage > 1)
            {
                result.Append(string.Format(PageUpDownTpl, new object[] { "pager-prev", "上一页", string.Format(link, (currentPage - 1)) }));
            }

            result.Append("<ol class=\"pager-numbers\">");
            int temp;

            if (currentPage == 1)
            {
                result.Append(string.Format(PageTextTpl, "pager-current", "1"));
            }
            else
            {
                result.Append(string.Format(PageLinkTpl, new object[] { string.Empty, "1", string.Format(link, "1") }));

                temp = Math.Max(currentPage - 2, 2);
                if (temp < currentPage)
                {
                    if (temp - 1 > 2)
                    {
                        result.Append(string.Format(PageTextTpl, "pager-ellipsis", "..."));
                    }
                    else if (temp > 2)
                    {
                        temp--;
                    }
                    for (var i = temp; i < currentPage; i++)
                    {
                        result.Append(
                            string.Format(PageLinkTpl, new object[] { string.Empty, i.ToString(), string.Format(link, i) })
                        );
                    }
                }

                result.Append(string.Format(PageTextTpl, "pager-current", currentPage));
            }

            temp = Math.Min(currentPage + 2, pageCount);
            if (temp > currentPage)
            {
                int i;
                var needEllipsis = false;
                if (pageCount - temp > 2)
                {
                    needEllipsis = true;
                }
                else if (pageCount - temp > 0)
                {
                    temp++;
                }

                for (i = currentPage + 1; i <= temp; i++)
                {
                    result.Append(
                        string.Format(PageLinkTpl, new object[] { string.Empty, i.ToString(), string.Format(link, i) }));
                }

                if (needEllipsis)
                {
                    result.Append(string.Format(PageTextTpl, "pager-ellipsis", "..."));
                }

                if (i <= pageCount)
                {
                    result.Append(string.Format(PageLinkTpl, new object[] { string.Empty, pageCount.ToString(), string.Format(link, pageCount) }));
                }
            }

            result.Append("</ol>");

            if (currentPage != pageCount)
            {
                result.Append(string.Format(PageUpDownTpl, new object[] { "pager-next", "下一页", string.Format(link, (currentPage + 1)) }));
            }

            result.Append("</div>");

            return result.ToString();
        }
    }
}
