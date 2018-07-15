using QiBuBlog.Util;
using QiBuBlog.Entity;
using System;
using System.Linq;

namespace QiBuBlog.Service
{
    public class SetupService : Singleton<SetupService>
    {
        private static EFRepositoryBase<Setup, object> _setup;

        private SetupService()
        {
            _setup = new EFRepositoryBase<Setup, object>();
        }

        public static void Validate(Setup setup)
        {
            if (setup.IsOpen != 1 && setup.IsOpen != 0)
            {
                throw new ArgumentException("网站开放参数错误");
            }
            if (string.IsNullOrEmpty(setup.SiteName))
            {
                throw new ArgumentException("网站名不能为空");
            }
            if (string.IsNullOrEmpty(setup.SiteDomain))
            {
                throw new ArgumentException("网站域名不能为空");
            }
            if (!string.IsNullOrEmpty(setup.ForbiddenIP))
            {
                var ips = setup.ForbiddenIP.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var ip in ips)
                {
                    if (!Validator.IsIP(ip))
                    {
                        throw new ArgumentException(ip + " 不是合法的IP地址");
                    }
                }
                setup.ForbiddenIP = String.Join(Environment.NewLine, ips);
            }
            if (setup.CommentLimit != 0 && setup.CommentLimit != 1 && setup.CommentLimit != 2)
            {
                throw new ArgumentException("评论开放参数错误");
            }
            if (setup.MinCommentSize > setup.MaxCommentSize)
            {
                throw new ArgumentException("最小评论字数不能大于最大评论字数");
            }
        }

        public static Setup GetSetup()
        {
            return _setup.Entities.FirstOrDefault();
        }

        public bool UpdateSetup(Setup model)
        {
            var result = false;
            try
            {
                _setup.Update(model);
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
