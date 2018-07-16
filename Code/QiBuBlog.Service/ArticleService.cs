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

        public PageList<Article> GetCustomerPageList(User user, string keyWord, PageSet pageSet)
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
    }
}
