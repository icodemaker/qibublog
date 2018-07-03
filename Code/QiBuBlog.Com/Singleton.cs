using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QiBuBlog.Com
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        private class SingleHolder
        {
            public static T Instance;

            static SingleHolder()
            {
                var constructor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
                if (constructor == null)
                {
                    throw new Exception(string.Format("类型“{0}”不存在无参私有构造函数。", typeof(T).FullName));
                }

                Instance = constructor.Invoke(null) as T;
            }
        }

        public static T Instance
        {
            get { return SingleHolder.Instance; }
        }
    }
}
