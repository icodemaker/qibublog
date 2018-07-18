using System;
using System.Collections.Generic;
using QiBuBlog.Entity;
using QiBuBlog.Util;
using System.Linq;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class ArticleService : Singleton<ArticleService>
    {
        private static EfRepositoryBase<Article, object> _article;

        private ArticleService()
        {
            _article = new EfRepositoryBase<Article, object>();
        }

        public DataPaging<Article> GetPageList(string categoryId, int currentPage)
        {
            try
            {
                var list = _article.Entities.ToList();
                var dataPaging = new DataPaging<Article>(list.Count, 10, currentPage);
                dataPaging.Data = dataPaging.RowCount > 0 ? list : new List<Article>();
                return dataPaging;
            }
            catch
            {
                throw new Exception("读取文章目录出错");
            }
        }

        public Article[] GetTopView(string categoryId, byte minWeight, byte pageSize)
        {
            return _article.Entities.Where(x => x.CategoryId == categoryId && x.Weight >= minWeight).Take(pageSize).ToArray();
        }

        public Article[] GetRecommends()
        {
            return _article.Entities.Where(x => x.Weight >= 200).ToArray();
        }

        public static bool IsExist(string articleId)
        {
            var article = _article.Find(x => x.ArticleId == articleId);
            return _article != null;
        }

        public bool CreateOrUpdate(Article model)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(model.ArticleId))
                {
                    _article.Update(model);
                }
                else
                {
                    _article.Insert(model);
                }
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
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
                        var model = _article.Find(x => x.ArticleId == id);
                        if (model != null)
                        {
                            model.State = 1;
                            _article.Update(model);
                            result = true;
                        }
                    }
                    else
                    {
                        _article.Delete(id);
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
