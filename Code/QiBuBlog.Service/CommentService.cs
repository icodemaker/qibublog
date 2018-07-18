using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class CommentService : Singleton<CommentService>
    {
        private static EfRepositoryBase<Comment, object> _comment;
        private CommentService()
        {
            _comment = new EfRepositoryBase<Comment, object>();
        }

        public void Validate(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.AuthorName))
            {
                throw new ArgumentException("称呼不能为空");
            }
            if (!string.IsNullOrEmpty(comment.Email) && !Validator.IsEmail(comment.Email))
            {
                throw new ArgumentException("电子邮箱格式错误");
            }
            if (string.IsNullOrEmpty(comment.Content))
            {
                throw new ArgumentException("内容不能为空");
            }

            var setup = SetupService.Instance.GetSetup();
            switch (setup.CommentLimit)
            {
                case 0:
                    throw new Exception("评论已关闭");
                case 2:
                    comment.Visibility = 1;
                    break;
                default:
                    break;
            }
            if (setup.MinCommentSize != 0 && comment.Content.Length < setup.MinCommentSize)
            {
                throw new ArgumentException("评论字数不能少于" + setup.MinCommentSize + "个字");
            }
            if (setup.MaxCommentSize != 0 && comment.Content.Length > setup.MaxCommentSize)
            {
                throw new ArgumentException("评论字数不能多于" + setup.MaxCommentSize + "个字");
            }
            if (setup.CommentInterval > 0 &&
                IsExists(comment.IP, DateTime.UtcNow.Subtract(new TimeSpan(0, 0, setup.CommentInterval))))
            {
                throw new Exception("请勿频繁发表评论");
            }

            if (!ArticleService.IsExist(comment.ArticleId))
            {
                throw new Exception("文章不存在");
            }

            if (!string.IsNullOrEmpty(comment.HomePage))
            {
                comment.HomePage = Validator.FixUrl(comment.HomePage);
            }

            comment.PostTime = DateTime.UtcNow;
        }

        private static bool IsExists(string ip, DateTime lastPostTime)
        {
            var comment = _comment.Find(x => x.IP == ip && x.PostTime == lastPostTime);
            return comment != null;
        }
    }
}
