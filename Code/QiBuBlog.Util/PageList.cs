using System.Collections.Generic;

namespace QiBuBlog.Util
{
    public class PageList<T>
    {
        public PageList()
        {
            Data = new List<T>();
        }

        public int PageIndex { get; set; }

        public int Total { get; set; }

        public List<T> Data { get; set; }
    }
}
