using QiBuBlog.Entity;
using QiBuBlog.Util;
using System.Linq;

namespace QiBuBlog.Service
{
    public class ArticleService : Singleton<ArticleService>
    {
        private ArticleService() { }

        public PageList<Article> GetCustomerPageList(User user, string keyWord, PageSet pageSet)
        {
            var db = new EFRepositoryBase<Article, object>();
                PageIndex = pageSet.PageIndex,
            var exp = new PredicatePack<Article>();

            return new PageList<Article>()
            {
                Data = db.Entities.ToList(),
                Total = db.Entities.Count()
            };
        }


    }
}
