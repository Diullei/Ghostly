using Ghostly;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //Test();
            //return;
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
                          <script src=""http://code.jquery.com/jquery-1.5.min.js""></script>
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
            browser.Visit("/browser/scripted",
                new BrowserOptions
                {
                    features = new BrowserOptions.Features
                    {
                        FetchExternalResources = true,
                        ProcessExternalResources = true
                    }
                }, (errors, window) =>
                {
                    if (window.document.title == "Nice")
                    {
                    }
                });

        }
    }
}
