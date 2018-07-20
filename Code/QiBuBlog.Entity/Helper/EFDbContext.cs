using System.Data.Entity;

namespace QiBuBlog.Entity.Helper
{
    public class EFDbContext<TEntity>
    {
        public readonly DbContext Instance = new QiBuBlogEntities();

        public EFDbContext()
        {
            //
        }
    }
}
