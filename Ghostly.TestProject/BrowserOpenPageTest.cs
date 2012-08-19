using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostly.TestProject
{
    [TestClass]
    [DeploymentItem("js", "js")]
    public class BrowserOpenPageTest
    {
        Browser _browser; 

        [TestInitialize]
        public void Before()
        {
            _browser = new Browser();
            _browser.Route.Interceptors.Add("/browser/scripted", () =>
                new HttpResponse
                {
                    Body = @"
                      <html>
                        <head>
                          <title>Whatever</title>
                          <script src=""/jquery.js""></script>
                        </head>
                        <body>
                          <h1 id=""h1"">Hello World</h1>
                          <script>
                            document.title = ""Nice"";
                            $(function() { $(""title"").text(""Awesome"") })
                          </script>
                          <script type=""text/x-do-not-parse"">
                            <p>this is not valid JavaScript</p>
                          </script>
                        </body>
                      </html>"
                });
        }

        [TestMethod]
        public void ShouldCreateHtmlDocument()
        {
            _browser.Visit("/browser/scripted", null, (errors, window) 
                => Assert.IsTrue(_browser.ExecScript<bool>("window.document instanceof jsdom.dom.level3.html.HTMLDocument")));
        }

        [TestMethod]
        public void ShouldLoadDocumentFromServer()
        {
            _browser.Visit("/browser/scripted", null, (errors, window)
                => Assert.AreEqual(window.document.getElementById("h1").innerHTML, "Hello World"));
        }

        [TestMethod]
        public void ShouldReturnStatusCodeOfLastRequest()
        {
            _browser.Visit("/browser/scripted", null, (errors, window)
                => Assert.AreEqual(_browser.StatusCode, 200));
        }
    }
}
