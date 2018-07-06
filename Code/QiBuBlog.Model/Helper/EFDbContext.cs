using System.Data.Entity;

namespace QiBuBlog.Model
{
    public class EFDbContext<TEntity>
    {
        public DbContext Instance = new Entities();

        public EFDbContext()
        {
            //
        }
    }
}
