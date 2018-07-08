namespace QiBuBlog.Util
{
    public class PageSet
    {
        public PageSet()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
