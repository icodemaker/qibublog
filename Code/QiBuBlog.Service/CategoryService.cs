using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using System.Linq;

namespace QiBuBlog.Service
{
    public class CategoryService : Singleton<CategoryService>
    {
        private static EFRepositoryBase<Category, object> _category;

        private CategoryService()
        {
            _category = new EFRepositoryBase<Category, object>();
        }

        public PageList<Category> GetCategroyPageList()
        {
            return new PageList<Category>()
            {
                PageIndex = 1,
                Data = _category.Entities.ToList(),
                Total = _category.Entities.Count()
            };
        }

        public bool CreateOrUpdateCategory(Category model)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(model.CategoryId))
                {
                    _category.Update(model);
                }
                else
                {
                    model.CategoryId = "";
                    _category.Insert(model);
                }
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public bool DeleteCategory(string id)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    _category.Delete(id);
                    result = true;
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
