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

        public static void Validate(User user)
        {
            if (string.IsNullOrEmpty(user.Password) || user.Password.Length < 6)
            {
                throw new ArgumentException("密码长度不能小于6位");
            }
            else if (user.Password.Length != 40)
            {
                user.Password = Encrypt.ToSHA1(user.Password);
            }

            if (!string.IsNullOrEmpty(user.Email) && !Validator.IsEmail(user.Email))
            {
                throw new ArgumentException("Email格式错误");
            }
            if (!string.IsNullOrEmpty(user.HomePage))
            {
                user.HomePage = Validator.FixUrl(user.HomePage);
            }
            if (!string.IsNullOrEmpty(user.QQ) && !Validator.IsQQ(user.QQ))
            {
                throw new ArgumentException("QQ号码格式错误");
            }
            if (string.IsNullOrEmpty(user.Nickname))
            {
                user.Nickname = user.UserName;
            }
        }

        public User GetUserById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new Exception("用户ID不能为空");
            }
            return _user.Find(x => x.UserId == userId);
        }

        public User GetUserForLogin(string userName, string password)
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

        public PageList<User> GetUserPageList()
        {
            return new PageList<User>()
            {
                PageIndex = 1,
                Data = _user.Entities.ToList(),
                Total = 10
            };
        }

        public bool CreateOrUpdateUser(User model)
        {
            return _user.Update(model) > 0;
        }

        public bool DeleteUser(string id)
        {
            return _user.Delete(id) > 0;
        }

        public bool ChangeStatus(string id, string lastIP)
        {
            var model = _user.Find(x => x.UserId == id);
            if (model == null) return _user.Update(model) > 0;
            model.LastActivity = DateTime.Now;
            model.LastIP = lastIP;
            return _user.Update(model) > 0;
        }
    }
}
