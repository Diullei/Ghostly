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
                var html0 = browser.ExecScript<string>("window.document.body.innerHTML");
                var html1 = browser.ExecScript<string>("window.document.getElementById('ghostly').innerHTML");
                var html2 = window.document.getElementById("ghostly").innerHTML;

                browser.Test.Assert(html1 == "Ghostly - C# Headless Browser!");
                browser.Test.Assert(html2 == "Ghostly - C# Headless Browser!");
            });
        }
    }
}
