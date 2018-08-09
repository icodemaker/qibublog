using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class CommentService : Singleton<CommentService>
    {
        private EFRepositoryBase<Comment, object> _comment;
        private CommentService()
        {
            _comment = new EFRepositoryBase<Comment, object>();
        }

        private bool IsExists(string ip, DateTime lastPostTime)
        {
            var comment = _comment.Find(x => x.IPAddress == ip && x.CreateTime == lastPostTime);
            return comment != null;
        }
    }
}
