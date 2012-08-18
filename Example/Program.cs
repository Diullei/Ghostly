using Ghostly;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var vm = new V8JsVM();

            vm.Exec(@"
var jsdom = require('jsdom/jsdom');

jsdom.env(""http://nodejs.org/dist/"", [
  'http://code.jquery.com/jquery-1.5.min.js'
],
function(errors, window) {
    process.console(""there have been"", window.$(""a"").length, ""nodejs releases!"");
});");
            return;
            /*
            // Exemple #1
            var browser = new Browser();

            browser.Route.Interceptors.Add("http://localhost:100/", 
                () => new HttpResponse
                {
                    Code = 200,
                    Message = "OK",
                    Body = @"
                        <!DOCTYPE HTML>
                        <html lang=""en-US"">
	                        <head>
		                        <meta charset=""UTF-8"">
		                        <title></title>
	                        </head>
	                        <body>
		                        <div id=""ghostly"">Ghostly - C# Headless Browser!</div>
	                        </body>
                        </html>"
                });

            
            browser.Visit("http://localhost:100/", null, () =>
            {
                var html0 = browser.ExecScript<string>("window.document.body.innerHTML");
                var html1 = browser.ExecScript<string>("window.document.getElementById('ghostly').innerHTML");
                var html2 = browser.Window.document.getElementById("ghostly").innerHTML;

                browser.Test.Assert(html1 == "Ghostly - C# Headless Browser!");
                browser.Test.Assert(html2 == "Ghostly - C# Headless Browser!");
            });
            */
            // Exemple #2
            var browser = new Browser();

            browser.Route.Interceptors.Add("http://localhost:1001/",
                () => new HttpResponse
                {
                    Code = 200,
                    Message = "OK",
                    //Body = "<html><head><script>var myObj = {name: \"Diullei Gomes\"};</script></head><body><div>Ghostly</div></body></html>"
                    Body = "<html><head><script>var myObj = {val: 1234};</script></head><body><div>Ghostly</div></body></html>"
                });

            browser.Visit("http://localhost:100/", null, () =>
            {
                var html0 = browser.ExecScript<string>("window.document.body.innerHTML");
                var name1 = browser.ExecScript<string>("window.myObj.val");
                var name2 = browser.Window.window.myObj.val;

                browser.Test.Assert(name1 == "Diullei Gomes");
                browser.Test.Assert(name2 == "Diullei Gomes");
            });
        }
    }
}
