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

            browser.Visit("http://diullei.github.com", () =>
            {
                result = browser.Run("document.title");

                result = result + "!";
            });
        }
    }
}
