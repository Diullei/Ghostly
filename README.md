## Ghostly - .NET Headless Browser

> UNDER DEVELOPMENT.

### Example

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

## TODO

* Include tests
* node.js files gateway
* Correct implementation of the httpRequest on C#
* ...

## External dependencies

* V8 Engine
* EnvJS

## Changelog

### v0.2.0 (2012-08-17)

* RhinoJS and IKMV.NET reference was removed. Code Rewritten to work with V8 engine. Started use of node.js files (under development).

### v0.1.0 (2012-08-13)

* Initial release. Running with RhinoJS ported to .NET with IKVM. Using EnvJS to emulate browser environment.

## License

Ghostly is distributed under the MIT license. [See license file here](https://raw.github.com/Diullei/Ghostly/master/LICENSE.txt) or below:

Copyright (c) 2012 by Diullei Gomes

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.