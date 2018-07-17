using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using System.Linq;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class CategoryService : Singleton<CategoryService>
    {
        private static EfRepositoryBase<Category, object> _category;

        private CategoryService()
        {
            _category = new EfRepositoryBase<Category, object>();
        }

        public PageList<Category> GetPageList()
        {
            return new PageList<Category>()
            {
                PageIndex = 1,
                Data = _category.Entities.ToList(),
                Total = _category.Entities.Count()
            };
        }

        public bool CreateOrUpdate(Category model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.CategoryId))
                {
                    _category.Update(model);
                }
                else
                {
                    _category.Insert(model);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(string id, bool isLogic = true)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    if (isLogic)
                    {
                        var model = _category.Find(x => x.CategoryId == id);
                        if (model != null)
                        {
                            _category.Update(model);
                            result = true;
                        }
                    }
                    else
                    {
                        _category.Delete(id);
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
