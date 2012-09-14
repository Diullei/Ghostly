using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostly.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var browser = new Browser();
            var result = "";

            browser.Visit("http://sdfsdf", () =>
            {
                result = browser.Run("document.title");
                Assert.AreEqual("Diullei Gomes", result);
            });
        }
    }
}
