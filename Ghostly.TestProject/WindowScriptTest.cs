using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostly.TestProject
{
    [TestClass]
    [DeploymentItem("js", "js")]
    [DeploymentItem("TestSrc", "TestSrc")]
    public class WindowScriptTest
    {
        Browser _browser;

        [TestInitialize]
        public void Before()
        {
            _browser = new Browser();
            _browser.Route.Interceptors.Add("/window/scripted", () =>
                new HttpResponse
                {
                    Body = @"
                        <html>
	                        <head>
                              <script type=""text/javascript"">
                                Object.prototype.a = 1;
                                hello = ""hello"";
                                window.bye = ""good"";
                                var abc = 123;
                                var localOnWindow = ""look at me, im on a window"";
                              </script>
                              <script type=""text/javascript"">
                                window.object = new Object();
                                hello += "" world"";
                                bye = bye + ""bye"";
                                window.confirmTheLocalIsOnTheWindow = localOnWindow;
                                (function() {
                                  var hidden = ""hidden"";
                                  window.exposed = hidden;
                                  this.imOnAWindow = true;
                                })();
                              </script>
	                          </head>
	                          <body></body>
                        </html>"
                });
        }

        [TestMethod]
        public void WindowShouldBeTheGlobalContext()
        {
            _browser.Visit("/window/scripted", (errors, window)
                => Assert.AreEqual("hello", window.hello));
        }

    }
}