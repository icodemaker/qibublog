using System.Data.Entity;

namespace QiBuBlog.Entity
{
    public class EFDbContext<TEntity>
    {
        public DbContext Instance = new QiBuBlogEntities();

        public EFDbContext()
        {
            //
        }
    }
}
