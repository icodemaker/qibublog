using System;
using System.Collections.Generic;

namespace QiBuBlog.Util
{
    public class DataPaging<T>
    {

        public readonly ushort PageSize;

        public readonly int CurrentPage;

        public readonly int RowCount;

        public readonly ushort PageCount;

        public DataPaging(int rowCount, ushort pageSize, int currentPage)
        {
            PageSize = pageSize > 0 ? pageSize : (ushort)1;
            CurrentPage = currentPage == 0 ? 1 : currentPage;
            RowCount = rowCount;
            PageCount = (ushort)(rowCount > 0 ? Math.Ceiling((decimal)rowCount / pageSize) : 0);

            if (CurrentPage < 0 || CurrentPage > PageCount)
            {
                CurrentPage = PageCount;
            }
        }

        public List<T> Data { get; set; }
    }
}
