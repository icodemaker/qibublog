using qibublog.model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QiBuBlog.Com
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
