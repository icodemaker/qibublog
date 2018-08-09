using QiBuBlog.Util;
using QiBuBlog.Entity;
using System;
using System.Linq;
using QiBuBlog.Entity.Helper;
using static System.String;

namespace QiBuBlog.Service
{
    public class SetupService : Singleton<SetupService>
    {
        private EFRepositoryBase<Setup, object> _setup;

        private SetupService()
        {
            _setup = new EFRepositoryBase<Setup, object>();
        }
        
        public Setup GetSetup()
        {
            return _setup.Entities.FirstOrDefault();
        }

        public bool UpdateSetup(Setup model)
        {
            try
            {
                _setup.Update(model);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
