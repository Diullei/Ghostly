using System;
using System.Collections.Generic;

namespace Ghostly
{
    public class Route
    {
        public Dictionary<string, Func<HttpResponse>> Interceptors = new Dictionary<string, Func<HttpResponse>>();
    }
}