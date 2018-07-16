using System;

namespace QiBuBlog.Entity.Helper
{
    public static class ObjectExtensions
    {
        public static T CastTo<T>(this object value)
        {
            object result;
            var type = typeof(T);
            try
            {
                if (type.IsEnum)
                {
                    result = Enum.Parse(type, value.ToString());
                }
                else if (type == typeof(Guid))
                {
                    result = Guid.Parse(value.ToString());
                }
                else
                {
                    result = Convert.ChangeType(value, type);
                }
            }
            catch
            {
                result = default(T);
            }

            return (T)result;
        }

        public static T CastTo<T>(this object value, T defaultValue)
        {
            object result;
            var type = typeof(T);
            try
            {
                result = type.IsEnum ? Enum.Parse(type, value.ToString()) : Convert.ChangeType(value, type);
            }
            catch
            {
                result = defaultValue;
            }
            return (T)result;
        }
    }
}
