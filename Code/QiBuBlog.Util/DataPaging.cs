using System.Collections.Generic;

namespace QiBuBlog.Util
{
    public class DataPaging<T>
    {
        public T SearchParams { get; set; }

        public List<T> List { get; set; }

        public string Pager { get; set; }
    }
}
