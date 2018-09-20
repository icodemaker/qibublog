using QiBuBlog.Entity;
using QiBuBlog.Entity.Helper;
using QiBuBlog.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QiBuBlog.Service
{
    public class CategoryService
    {
        private readonly EFRepositoryBase<Category, object> _category = new EFRepositoryBase<Category, object>();

        public DataPaging<Category> GetPageList()
        {
            return null;
        }

        public List<Category> GetList()
        {
            return _category.Entities.ToList();
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
