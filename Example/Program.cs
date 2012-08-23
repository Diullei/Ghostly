using Ghostly;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            return;
            var browser = new Browser();
            browser.Init();

            var html = @"
            <!DOCTYPE HTML>
            <html lang=""en-US"">
	            <head>
		            <meta charset=""UTF-8"">
		            <title></title>
	            </head>
	            <body>
		            <div id=""ghostly"">Ghostly - C# Headless Browser!</div>
	            </body>
            </html>";

            browser.Visit(html, null, (errors, window) =>
            {
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
            browser.Route.Interceptors.Add("/browser/scripted", () =>
                new HttpResponse
                {
                    Body = @"
                      <html>
                        <head>
                          <title>Whatever</title>
                          <script src=""http://ajax.googleapis.com/ajax/libs/jquery/1.6.0/jquery.js""></script>
                        </head>
                        <body>
                          <h1 id=""h1"">Hello World</h1>
                          <script>
                                    console.log(""xxxxxxx""); 
                            document.title = ""Nice"";
//comment
$(function(){$(""title"").text(""Awesome"")});
                                window.onload = function(e){ 
                                    console.log(""document.onload""); 
                                }
                            </script>
                          <script type=""text/x-do-not-parse"">
                            <p>this is not valid JavaScript</p>
                          </script>
                        </body>
                      </html>"
                });
            browser.Visit("/browser/scripted",
                null, (errors, window) =>
                    {
                        if (window.document.title == "Nice")
                        {
                        }
                        else if (window.document.title == "Awesome")
                        {
                            var t = browser.ExecScript<string>("window.jQuery('title').text()");
                            var jq = window.jQuery("title").text();
                            var name = window.document.title;
                        }
                        else
                        {
                            
                        }
                    });

        }
    }
}
