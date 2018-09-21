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
            if (string.IsNullOrWhiteSpace(id)) return result;
            var exp = new PredicatePack<Menu>();
            exp.PushAnd(x => x.MenuId == id);
            exp.PushAnd(x => x.CategoryId != null);
            var exist = _menu.Find(exp);
            return exist != null ? exist.CategoryId : result;
        }

        public List<Menu> GetList()
        {
            return _menu.Entities.Where(x => x.Status == 101).OrderBy(x => x.Sort).ToList();
        }
    }
}
