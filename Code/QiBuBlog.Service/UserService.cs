using QiBuBlog.Entity;
using QiBuBlog.Entity.Helper;
using QiBuBlog.Util;
using System;
using System.Linq;
using System.Web;

namespace QiBuBlog.Service
{
    public class UserService
    {
        private readonly EFRepositoryBase<User, object> _user = new EFRepositoryBase<User, object>();

        public User UserLogin(string userName, string password)
        {
            return string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password)
                ? throw new Exception("用户名或密码不能为空")
                : _user.Find(x => x.UserName == userName && x.Password == password);
        }

        public DataPaging<User> GetPageList(User parameters, int currentPage, int pageSize)
        {
            var exp = new PredicatePack<User>();
            if (!string.IsNullOrEmpty(parameters.UserName))
            {
                exp.PushAnd(x => x.UserName.Contains(parameters.UserName));
            }
            if (!string.IsNullOrEmpty(parameters.DisplayName))
            {
                exp.PushAnd(x => x.DisplayName.Contains(parameters.DisplayName));
            }
            var source = _user.Entities.Where(exp);
            var list = source.OrderBy(x => true).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var totalRecord = source.Count();
            return new DataPaging<User>()
            {
                SearchParams = parameters,
                List = list,
                Pager = totalRecord < 1 ? string.Empty : (new HtmlPager<User>(HttpContext.Current.Request.Path.ToLower(), parameters))
                .GenerateCode(totalRecord / pageSize, currentPage)
            };
        }
    }
}
