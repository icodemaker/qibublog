using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QiBuBlog.Service;

namespace QiBuBlog.UnitTest
{
    [TestClass]
    public class ArticleUnitTest
    {
        [TestMethod]
        public void Add()
        {
            var res = new SetupService().GetSetup();
        }

        [TestMethod]
        public void PageList()
        {

        }

        [TestMethod]
        public void Del()
        {

        }
    }
}
