Ghostly - c# headless browser
=============================

Under construction.

Example:

```csharp
using Ghostly;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            var browser = new Browser();
            browser.Visit("http://localhost:100/", null, () =>
            {
                var html = browser.ExecScript<string>("document.getElementById('div_id').innerHTML");
            });
        }
    }
}
```