using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Ghostly
{
    public class Http
    {
        private readonly Route _route;

        public Http(Route route)
        {
            _route = route;
        }

        public Request Get(string hostname, Dictionary<string, object> headers)
        {
            return new Request(_route, hostname, headers);
        }
    }
}
