using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class UserService
    {
        private readonly EFRepositoryBase<User, object> _user = new EFRepositoryBase<User, object>();

        public User GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("用户ID不能为空");
            }
            return _user.Find(x => x.UserId == userId);
        }

        public User UserLogin(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new Exception("用户名不能为空");
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("密码不能为空");
            }
            return _user.Find(x => x.UserName == userName && x.Password == password);
        }

        public DataPaging<User> GetPageList(User queryParams, int currentPage, int pageSize)
        {
            var exp = new PredicatePack<User>();
            if (!string.IsNullOrEmpty(queryParams.UserName))
            {
                exp.PushAnd(x => x.UserName.Contains(queryParams.UserName));
            }
            if (!string.IsNullOrEmpty(queryParams.DisplayName))
            {
                exp.PushAnd(x => x.DisplayName.Contains(queryParams.DisplayName));
            }
            var source = _user.Entities.Where(exp);
            var list = source.OrderBy(x => true).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            var totalRecord = source.Count();
            return new DataPaging<User>()
            {
                SearchParams = queryParams,
                List = list,
                Pager = totalRecord < 1 ? string.Empty 
                : (new HtmlPager<User>(HttpContext.Current.Request.Path.ToLower(), queryParams))
                .GenerateCode(totalRecord / pageSize, currentPage)
            };
        }

        public bool CreateOrUpdate(User model)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(model.UserId))
                {
                    _user.Update(model);
                }
                else
                {
                    _user.Insert(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }

        public bool Delete(string id, bool isLogic = true)
        {
            var result = false;
            try
            {
                if (!string.IsNullOrWhiteSpace(id))
                {
                    if (isLogic)
                    {
                        var model = _user.Find(x => x.UserId == id);
                        if (model != null)
                        {
                            _user.Update(model);
                            result = true;
                        }
                    }
                    else
                    {
                        _user.Delete(id);
                        result = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}
