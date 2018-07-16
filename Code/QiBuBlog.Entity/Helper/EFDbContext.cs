using System.Data.Entity;

namespace QiBuBlog.Entity.Helper
{
    public class EfDbContext<TEntity>
    {
        public readonly DbContext Instance = new QiBuBlogEntities();

        public EfDbContext()
        {
            //
        }
    }
}
