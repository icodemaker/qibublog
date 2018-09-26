using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QiBuBlog.Service;

namespace QiBuBlog.UnitTest
{
    [TestClass]
    public class ArticleUnitTest
    {
        [TestMethod]
        public void Test()
        {
            new ArticleService().GetGroupList();
        }
    }
}
