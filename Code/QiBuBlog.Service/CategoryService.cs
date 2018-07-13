using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using System.Linq;

namespace QiBuBlog.Service
{
    public class CategoryService : Singleton<CategoryService>
    {
        private readonly EFRepositoryBase<Category, object> category;

        public CategoryService()
        {
            category = new EFRepositoryBase<Category, object>();
        }

        public PageList<Category> GetCategroyPageList()
        {
            return new PageList<Category>()
            {
                PageIndex = 1,
                Data = category.Entities.ToList(),
                Total = category.Entities.Count()
            };
        }

        public bool CreateOrUpdateCategory(Category model)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(model.CategoryId))
                {
                    category.Update(model);
                }
                else
                {
                    model.CategoryId = "";
                    category.Insert(model);
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
                    category.Delete(id);
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
