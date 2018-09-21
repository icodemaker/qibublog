using QiBuBlog.Entity;
using QiBuBlog.Entity.Helper;
using QiBuBlog.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QiBuBlog.Service
{
    public class CategoryService
    {
        private readonly EFRepositoryBase<Category, object> _category = new EFRepositoryBase<Category, object>();

        public DataPaging<Category> GetPageList(Category parameters, int currentPage, int pageSize)
        {
            var exp = new PredicatePack<Category>();
            var source = _category.Entities.Where(exp);
            var list = source.OrderBy(x => true).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var totalRecord = source.Count();
            return new DataPaging<Category>
            {
                SearchParams = parameters,
                List = list,
                Pager = totalRecord < 1 ? string.Empty : new HtmlPager<Category>(HttpContext.Current.Request.Path.ToLower(), parameters)
                    .GenerateCode(totalRecord / pageSize, currentPage)
            };
        }

        public List<Category> GetList()
        {
            return _category.Entities.ToList();
        }

        public bool CreateOrUpdate(Category model)
        {
            return !string.IsNullOrWhiteSpace(model.CategoryId)
                ? _category.Update(model) > 0
                : _category.Insert(model) > 0;
        }

        public void Delete(string id, bool isLogic = true)
        {
            if (string.IsNullOrWhiteSpace(id)) return;
            if (isLogic)
            {
                var model = _category.Find(x => x.CategoryId == id);
                if (model != null)
                {
                    _category.Update(model);
                }
            }
            else
            {
                _category.Delete(id);
            }
        }
    }
}
