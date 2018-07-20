using QiBuBlog.Entity;
using QiBuBlog.Entity.Helper;
using QiBuBlog.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QiBuBlog.Service
{
    public class MenuService : Singleton<MenuService>
    {
        private static EFRepositoryBase<Menu, object> _menu;

        private MenuService()
        {
            _menu = new EFRepositoryBase<Menu, object>();
        }

        public List<Menu> GetList()
        {
            try
            {
                var list = _menu.Entities.ToList();

                return list;
            }
            catch
            {
                throw new Exception("读取文章目录出错");
            }
        }
    }
}
