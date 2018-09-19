using System;
using System.Collections.Generic;
using QiBuBlog.Entity;
using QiBuBlog.Util;
using System.Linq;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class ArticleService
    {
        private EFRepositoryBase<Article, object> _article = new EFRepositoryBase<Article, object>();
        private EFRepositoryBase<ArticleListView, object> _articleView = new EFRepositoryBase<ArticleListView, object>();

        public DataPaging<ArticleListView> GetPageList(string categoryId, int currentPage, bool isIndex)
        {
            try
            {
                var exp = new PredicatePack<ArticleListView>();

                if (!isIndex)
                {
                    exp.PushAnd(x => x.CategoryId == categoryId);
                }

                var list = _articleView.Entities.Where(exp).ToList();

                return new DataPaging<ArticleListView>()
                {
                    CurrentPage = currentPage,
                    Data = list,
                    TotalRecord = list.Count
                };
            }
            catch
            {
                throw new Exception("读取文章目录出错");
            }
        }

        public GetArticleById_Result GetArticleById(string id)
        {
            var result = new GetArticleById_Result();
            var db = new QiBuBlogEntities();
            if (!string.IsNullOrWhiteSpace(id))
            {
                result = db.GetArticleById(id).FirstOrDefault();
            }
            return result;
        }

        public Article[] GetTopView(string categoryId, byte minWeight, byte pageSize)
        {
            return _article.Entities.Where(x => x.CategoryId == categoryId && x.Weight >= minWeight).Take(pageSize).ToArray();
        }

        public Article[] GetRecommends()
        {
            return _article.Entities.Where(x => x.Weight >= 200).ToArray();
        }

        public bool IsExist(string articleId)
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
