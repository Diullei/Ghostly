using System.Collections.Generic;

namespace Ghostly
{
    public class BrowserOptions
    {
        public class Features
        {
            public bool FetchExternalResources { get; set; }
            public bool ProcessExternalResources { get; set; }
        }

        public Features features { get; set; }
    }
}