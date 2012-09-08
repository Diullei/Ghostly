using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Ghostly
{
    public class Request
    {
        private readonly Route _route;
        private readonly string _uri;
        private readonly Dictionary<string, object> _headers;

        public Request(Route route, string uri, Dictionary<string, object> headers)
        {
            _route = route;
            _uri = uri;
            _headers = headers;
        }

        public Response GetResponse()
        {
            HttpResponse route = null;

            if (_route.Interceptors.ContainsKey(_uri))
            {
                route = _route.Interceptors
                    .Where(
                        i =>
                        i.Key == _uri)
                        .First().Value.Invoke();
            }
            else
            {
                // TODO
                var wr = WebRequest.Create(_uri);
                route = new HttpResponse
                            {
                                Code = 200,
                                Message = "OK",
                                Body = _uri.Contains("qunit-git")?  
                                    Util.GetResource("qunit-git.js")
                                    :new StreamReader(wr.GetResponse().GetResponseStream()).ReadToEnd()
                            };
            }

            return new Response(route.Code, route.Message, route.Body);
        }
    }
}