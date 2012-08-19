using Ghostly;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
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
    }
}
