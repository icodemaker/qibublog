using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using System.Linq;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class UserService : Singleton<UserService>
    {
        private EFRepositoryBase<User, object> _user;

        private UserService()
        {
            _user = new EFRepositoryBase<User, object>();
        }

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

        public DataPaging<User> GetPageList()
        {
            return new DataPaging<User>()
            {
                CurrentPage = 1,
                TotalRecord = _user.Entities.Count(),
                Data = _user.Entities.ToList()
            };
        }

        public bool CreateOrUpdate(User model)
        {
            var result = false;
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
                result = true;
            }
            catch (Exception)
            {
                throw;
            }
            return result;
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
