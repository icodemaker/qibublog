using QiBuBlog.Entity;
using QiBuBlog.Entity.Helper;
using QiBuBlog.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QiBuBlog.Service
{
    public class MenuService
    {
        private readonly EFRepositoryBase<Menu, object> _menu = new EFRepositoryBase<Menu, object>();

        public string GetMenuCategoryId(string id)
        {
            var result = string.Empty;
            if (!string.IsNullOrWhiteSpace(id))
            {
                var exp = new PredicatePack<Menu>();
                exp.PushAnd(x => x.MenuId == id);
                exp.PushAnd(x => x.CategoryId != null);
                var exist = _menu.Find(exp);
                if (exist != null)
                {
                    return exist.CategoryId;
                }
            }
            return result;
        }

        public List<Menu> GetList()
        {
            try
            {
                var list = _menu.Entities.Where(x => x.Status == 101).OrderBy(x  => x.Sort).ToList();

                return list;
            }
            catch
            {
                throw new Exception("读取文章目录出错");
            }
        }
    }
}
