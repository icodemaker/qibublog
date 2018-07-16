namespace QiBuBlog.Util
{
    public abstract class PageSet
    {
        protected PageSet()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public int PageIndex { get; set; }

        private int PageSize { get; set; }
    }
}
