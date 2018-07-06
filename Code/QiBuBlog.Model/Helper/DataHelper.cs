using System;

namespace QiBuBlog.Model
{
    internal static class DataHelper
    {
        public static string GetSqlExceptionMessage(int number)
        {
            string msg = string.Empty;
            switch (number)
            {
                case 2:
                    msg = "连接数据库超时，请检查网络连接或者数据库服务器是否正常。";
                    break;
                case 17:
                    msg = "SqlServer服务不存在或拒绝访问。";
                    break;
                case 17142:
                    msg = "SqlServer服务已暂停，不能提供数据服务。";
                    break;
                case 2812:
                    msg = "指定存储过程不存在。";
                    break;
                case 208:
                    msg = "指定名称的表不存在。";
                    break;
                case 4060: //数据库无效。
                    msg = "所连接的数据库无效。";
                    break;
                case 18456: //登录失败
                    msg = "使用设定的用户名与密码登录数据库失败。";
                    break;
                case 547:
                    msg = "外键约束，无法保存数据的变更。";
                    break;
                case 2627:
                    msg = "主键重复，无法插入数据。";
                    break;
                case 2601:
                    msg = "未知错误。";
                    break;
            }
            return msg;
        }

        #region EF使用
        public static void CheckArgument(object arg, string argName, bool canZero = false)
        {
            if (arg == null)
            {
                ArgumentNullException e = new ArgumentNullException(argName);
                throw ThrowComponentException(string.Format("参数 {0} 为空引发异常。", argName), e);
            }
            Type type = arg.GetType();
            if (type.IsValueType && type.IsNumeric())
            {
                bool flag = !canZero ? arg.CastTo(0.0) <= 0.0 : arg.CastTo(0.0) < 0.0;
                if (flag)
                {
                    ArgumentOutOfRangeException e = new ArgumentOutOfRangeException(argName);
                    throw ThrowComponentException(string.Format("参数 {0} 不在有效范围内引发异常。具体信息请查看系统日志。", argName), e);
                }
            }
            if (type == typeof(Guid) && (Guid)arg == Guid.Empty)
            {
                ArgumentNullException e = new ArgumentNullException(argName);
                throw ThrowComponentException(string.Format("参数{0}为空Guid引发异常。", argName), e);
            }
        }

        public static ComponentException ThrowComponentException(string msg, Exception e = null)
        {
            if (string.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (string.IsNullOrEmpty(msg))
            {
                msg = "未知组件异常，详情请查看日志信息。";
            }
            return e == null ? new ComponentException(string.Format("组件异常：{0}", msg)) : new ComponentException(string.Format("组件异常：{0}", msg), e);
        }

        public static DataAccessException ThrowDataAccessException(string msg, Exception e = null)
        {
            if (string.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (string.IsNullOrEmpty(msg))
            {
                msg = "未知数据访问层异常，详情请查看日志信息。";
            }
            return e == null
                ? new DataAccessException(string.Format("数据访问层异常：{0}", msg))
                : new DataAccessException(string.Format("数据访问层异常：{0}", msg), e);
        }

        public static BusinessException ThrowBusinessException(string msg, Exception e = null)
        {
            if (string.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (string.IsNullOrEmpty(msg))
            {
                msg = "未知业务逻辑层异常，详情请查看日志信息。";
            }
            return e == null ? new BusinessException(string.Format("业务逻辑层异常：{0}", msg)) : new BusinessException(string.Format("业务逻辑层异常：{0}", msg), e);
        }
        #endregion
    }
}
