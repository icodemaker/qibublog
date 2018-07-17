using System;
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

        public PageList<Article> GetPageList(User user, string keyWord, PageSet pageSet)
        {
            var exp = new PredicatePack<Article>();

            return new PageList<Article>()
            {
                PageIndex = pageSet.PageIndex,
                Data = _article.Entities.ToList(),
                Total = _article.Entities.Count()
            };
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

        public bool DeleteLogic(string id, bool isLogic = true)
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
