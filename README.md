## Ghostly - .NET Headless Browser

Under construction.

Example:

```csharp
class Program
{
	static void Main(string[] args)
	{
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
			var html1 = browser.ExecScript<string>("window.document.getElementById('ghostly').innerHTML");
			var html2 = browser.Window.document.getElementById("ghostly").innerHTML;

			browser.Test.Assert(html1 == "Ghostly - C# Headless Browser!");
			browser.Test.Assert(html2 == "Ghostly - C# Headless Browser!");
		});
	}
}
```