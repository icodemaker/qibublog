using System;
using System.Collections.Generic;
using QiBuBlog.Entity;
using QiBuBlog.Util;
using System.Linq;
using System.Web;
using QiBuBlog.Entity.Helper;
using QiBuBlog.Entity.DTO;

namespace QiBuBlog.Service
{
    public class ArticleService
    {
        private readonly EFRepositoryBase<Article, object> _article = new EFRepositoryBase<Article, object>();

        public DataPaging<ArticleListView> GetPageList(ArticleListView parameters, int currentPage, int pageSize, bool isIndex = true)
        {
            var _articleView = new EFRepositoryBase<ArticleListView, object>();
            var exp = new PredicatePack<ArticleListView>();
            if (!isIndex)
            {
                exp.PushAnd(x => x.CategoryId == parameters.CategoryId);
            }
            exp.PushAnd(x => x.Status == 101);
            var source = _articleView.Entities.Where(exp);
            var list = source.OrderByDescending(x => x.CreateTime).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var totalRecord = source.Count();
            return new DataPaging<ArticleListView>
            {
                SearchParams = parameters,
                List = list,
                Pager = totalRecord < 1 ? string.Empty : new HtmlPager<ArticleListView>(HttpContext.Current.Request.Path.ToLower(), parameters)
                .GenerateCode(totalRecord / pageSize, currentPage)
            };
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

        public List<Article> GetTopView(byte minWeight, byte pageSize)
        {
            return _article.Entities.Where(x => x.Weight >= minWeight && x.Status == 101).OrderByDescending(x => x.ViewCount).Take(pageSize).ToList();
        }

        public List<Article> GetRecommends()
        {
            return _article.Entities.Where(x => x.Weight >= 200 && x.Status == 101).ToList();
        }

        public List<ArticleGroup> GetGroupList()
        {
            var sql = @"SELECT TOP (100) CONVERT(VARCHAR(7), CreateTime, 120) AS GroupName, COUNT(1) AS Amount
                        FROM dbo.Article
                        GROUP BY CONVERT(VARCHAR(7), CreateTime, 120),Status
                        HAVING (Status = 101)
                        ORDER BY GroupName DESC";
            return _article.ExcSql<ArticleGroup>(sql).ToList();
        }

        public bool IsExist(string articleId)
        {
            var article = _article.Find(x => x.ArticleId == articleId);
            return _article != null;
        }

        public void CreateOrUpdate(Article model)
        {
            if (string.IsNullOrWhiteSpace(model.Summary))
            {
                var summaryLength = model.SummarySize > model.Content.Length ? model.Content.Length : model.SummarySize;
                model.Summary = model.Content.Substring(0, summaryLength);
            }
            if (!string.IsNullOrWhiteSpace(model.ArticleId))
            {
                _article.Update(model);
            }
            else
            {
                model.ArticleId = Helper.CreateGuidWithNoSplit();
                _article.Insert(model);
            }
        }

        public void Delete(string id, bool isLogic = true)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (isLogic)
                {
                    var model = _article.Find(x => x.ArticleId == id);
                    if (model != null)
                    {
                        model.Status = 103;
                        _article.Update(model);
                    }
                }
                else
                {
                    _article.Delete(id);
                }
            }
        }
    }
}
