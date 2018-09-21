using QiBuBlog.Entity;
using QiBuBlog.Entity.Helper;
using System;

namespace QiBuBlog.Service
{
    public class CommentService 
    {
        private readonly EFRepositoryBase<Comment, object> _comment = new EFRepositoryBase<Comment, object>();

        private bool IsExists(string ip, DateTime lastPostTime)
        {
            var comment = _comment.Find(x => x.IPAddress == ip && x.CreateTime == lastPostTime);
            return comment != null;
        }
    }
}
