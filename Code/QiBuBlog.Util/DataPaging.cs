using System.Collections.Generic;

namespace QiBuBlog.Util
{
    public class DataPaging<T>
    {
        public List<T> List { get; set; }

        public string Pager { get; set; }
    }
}
