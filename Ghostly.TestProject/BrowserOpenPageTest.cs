// ===============================================================================
// Ghostly - .NET Headless Browser
// https://github.com/Diullei/Ghostly
//
// BrowserOpenPageTest.cs
//
// Browser test class
// ===============================================================================
// SINCE VERSION: 0.2.1
// ===============================================================================
// Copyright (c) 2012 by Diullei Gomes
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
// USE OR OTHER DEALINGS IN THE SOFTWARE.
// ===============================================================================
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Ghostly.TestProject
{
    [TestClass]
    [DeploymentItem("js", "js")]
    [DeploymentItem("TestSrc", "TestSrc")]
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
                          </script>
                          <script type=""text/x-do-not-parse"">
                            <p>this is not valid JavaScript</p>
                          </script>
                        </body>
                      </html>"
                });

            _browser.Route.Interceptors.Add("/browser/scripted2", () =>
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
                            //comment
                            $(function() { $(""title"").text(""Awesome"") })
                          </script>
                          <script type=""text/x-do-not-parse"">
                            <p>this is not valid JavaScript</p>
                          </script>
                        </body>
                      </html>"
                });

            _browser.Route.Interceptors.Add("/jquery.js", () => new HttpResponse { Body = File.ReadAllText("TestSrc\\jQuery-1.6.0.js") });
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

        [TestMethod]
        public void ShouldHaveAParent()
        {
            _browser.Visit("/browser/scripted", null, (errors, window)
                => Assert.IsNotNull(window.parent));
        }

        [TestMethod]
        public void ShouldExecuteInlineScriptBlocks()
        {
            _browser.Visit("/browser/scripted", null, (errors, window)
                => Assert.AreEqual(window.document.title, "Nice"));
        }

        [TestMethod]
        public void ShouldLoadExternalScripts()
        {
            _browser.Visit("/browser/scripted2", null, (errors, window)
                => Assert.IsNotNull(_browser.Js.window.jQuery));
        }

        [TestMethod]
        public void ShouldRunJQueryOnReady()
        {
            _browser.Visit("/browser/scripted2", null, (errors, window)
                => Assert.AreEqual(window.document.title, "Awesome"));
        }

        [TestMethod]
        public void ShouldIndicateSuccess()
        {
            _browser.Visit("/browser/scripted2", null, (errors, window)
                => Assert.IsTrue(_browser.Success));
        }
    }
}
