using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiBuBlog.Com
{
    /// <summary>
    /// 分页参数设置
    /// </summary>
    public class PageSet
    {
        public PageSet()
        {
            this.page = 1;
            this.rows = 20;
        }

        /// <summary>
        /// 当前页面序号
        /// </summary>
        public int page { get; set; }

        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int rows { get; set; }
    }
}
