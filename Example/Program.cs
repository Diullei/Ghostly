using System.Text.RegularExpressions;
using Ghostly;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
            return;

            var browser = new Browser();
            browser.Init();

            var html = @"
            <!DOCTYPE HTML>
            <html lang=""en-US"">
	            <head>
                    <script src=""http://ajax.googleapis.com/ajax/libs/jquery/1.8.0/jquery.js""></script>
		            <meta charset=""UTF-8"">
		            <title>Hello</title>
                    <script>
                        console.log(""logging...""); 
                        document.title = ""Nice"";
                        //comment
                        $(function(){$(""title"").text(""Awesome"")});
                        window.onload = function(e){ 
                            console.log(""logging..onload""); 
                        }
                    </script>
                    <script type=""text/x-do-not-parse"">
                        <p>this is not valid JavaScript</p>
                    </script>
	            </head>
	            <body>
		            <div id=""ghostly"">Ghostly - C# Headless Browser!</div>
	            </body>
            </html>";

            browser.Visit(html, (errors, window) =>
            {
                browser.ExecScript("Log.breakpoint();");

                var html0 = browser.ExecScript<string>("window.document.body.innerHTML");
                var html1 = browser.ExecScript<string>("window.document.getElementById('ghostly').innerHTML");
                var html2 = window.document.getElementById("ghostly").innerHTML;

                browser.Test.Assert(html1 == "Ghostly - C# Headless Browser!");
                browser.Test.Assert(html2 == "Ghostly - C# Headless Browser!");
            });
        }

        public static void Test()
        {
            var browser = new Browser();
            browser.Init();
            
            var html = @"
                      <html>
                        <head>
                          <title>Whatever</title>
                          <script src=""http://code.jquery.com/qunit/qunit-git.js""></script>
                          <script src=""https://raw.github.com/Diullei/DocJS/master/docjs.js""></script>
                          <script src=""https://raw.github.com/Diullei/DocJS/master/tests/test.js""></script>
                        </head>
                        <body>
<p id=""qunit-testresult""></p>
<h2 id=""qunit-banner""></h2>
<h2 id=""qunit-userAgent""></h2>
<div id=""qunit-testrunner-toolbar""></div>
<div id=""qunit-fixture"">test markup, will be hidden</div>
<ol id=""qunit-tests""></ol>
                          <h1 id=""h1"">Hello World</h1>
                          <script>
                            document.title = ""Nice"";
                          </script>
                          <script type=""text/x-do-not-parse"">
                            <p>this is not valid JavaScript</p>
                          </script>
                        </body>
                      </html>";

            browser.Visit(html, (errors, window) =>
            {
                browser.ExecScript("Log.breakpoint(window);");
            });
        }

        public static void Test2()
        {
            var browser = new Browser();
            browser.Init();

            var html = @"
                      <html>
                        <head>
                          <title></title>
                        </head>
                        <body>
                          <script>
                            window.location = ""http://www.google.com"";
                          </script>
                        </body>
                      </html>";

            browser.Visit(html, (errors, window) =>
            {
                browser.ExecScript("Log.breakpoint(window);");
            });
        }
    }
}
