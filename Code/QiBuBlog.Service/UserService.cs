using QiBuBlog.Entity;
using QiBuBlog.Util;
using System;
using System.Linq;
using QiBuBlog.Entity.Helper;

namespace QiBuBlog.Service
{
    public class UserService : Singleton<UserService>
    {
        private static EfRepositoryBase<User, object> _user;

        private UserService()
        {
            _user = new EfRepositoryBase<User, object>();
        }

        public User GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("用户ID不能为空");
            }
            return _user.Find(x => x.UserId == userId);
        }

        public static User UserLogin(string userName, string password)
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
            return null;
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

        //public bool ChangeStatus(string id, string lastIP)
        //{
        //    var model = _user.Find(x => x.UserId == id);
        //    if (model == null) return _user.Update(model) > 0;
        //    model.LastActivity = DateTime.Now;
        //    model.LastIP = lastIP;
        //    return _user.Update(model) > 0;
        //}
    }
}
