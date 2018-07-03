using qibublog.com;
using qibublog.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qibublog.service
{
    public class ArticleService : Singleton<ArticleService>
    {
        private ArticleService() { }

        public PageList<Article> GetCustomerPageList(User user, string keyWord, PageSet pageSet)
        {
            var db = new EFRepositoryBase<Article, object>();
            var exp = new PredicatePack<Article>();

            return new PageList<Article>()
            {
                page = pageSet.page,
                rows = 1,
                total = 1
            };
        }

    }
}
