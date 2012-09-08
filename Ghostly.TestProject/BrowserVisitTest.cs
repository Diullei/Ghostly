using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostly.TestProject
{
    [TestClass]
    [DeploymentItem("js", "js")]
    [DeploymentItem("TestSrc", "TestSrc")]
    public class BrowserVisitTest
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
                          </script>
                          <script type=""text/x-do-not-parse"">
                            <p>this is not valid JavaScript</p>
                          </script>
                        </body>
                      </html>"
                });

            _browser.Route.Interceptors.Add("/browser/scripted3", () =>
                new HttpResponse
                {
                    Body = @"
                      <html>
                        <head>
                          <title></title>
                        </head>
                        <body>
                          <script>
                            window.location = ""http://www.google.com"";
                          </script>
                        </body>
                      </html>"
                });

            _browser.Route.Interceptors.Add("/jquery.js", () => new HttpResponse { Body = File.ReadAllText("TestSrc\\jQuery-1.6.0.js") });
        }

        [TestMethod]
        public void ShouldCallCallbackWithoutError()
        {
            _browser.Visit("/browser/scripted", (errors, window)
                => Assert.IsNull(errors));
        }

        [TestMethod]
        public void ShouldPassWindowToCallback()
        {
            _browser.Visit("/browser/scripted", (errors, window)
                => Assert.IsNotNull(window));
        }

        [TestMethod]
        public void WindowLocationTest()
        {
            _browser.Visit("/browser/scripted3", (errors, window)
                =>
                                                    {
                                                        
                                                    });
        }
    }
}