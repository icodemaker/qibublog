using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qibublog.com
{
    /// <summary>
    /// 分页后的数据集合
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class PageList<T>
    {
        public PageList()
        {
            rows = new List<T>();
        }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 分页数据
        /// </summary>
        public List<T> rows { get; set; }

        /// <summary>
        /// 底部合计
        /// </summary>
        public object footer { get; set; }

        /// <summary>
        /// 子集
        /// </summary>
        public object children { get; set; }

        /// <summary>
        /// 第二子集
        /// </summary>
        public object secendChildren { get; set; }
    }
}
