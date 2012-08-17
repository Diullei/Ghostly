using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Ghostly
{
    public class Request
    {
        private readonly Route _route;
        private readonly string _hostname;
        private readonly int _port;
        private readonly string _path;
        private readonly Dictionary<string, object> _headers;

        public Request(Route route, string hostname, int port, string path, Dictionary<string, object> headers)
        {
            _route = route;
            _hostname = hostname;
            _port = port;
            _path = path;
            _headers = headers;
        }

        public Response GetResponse()
        {
            HttpResponse route = null;

            if (_route.Interceptors.ContainsKey(string.Format("http://{0}{1}{2}", _hostname, (_port == 80 ? "" : ":" + _port), _path)))
            {
                route = _route.Interceptors
                    .Where(
                        i =>
                        i.Key == string.Format("http://{0}{1}{2}", _hostname, (_port == 80 ? "" : ":" + _port), _path))
                        .First().Value.Invoke();
                // TODO-BUG
                route.Body = route.Body.Replace(System.Environment.NewLine, "");
            }
            else
            {
                // TODO
                var wr = WebRequest.Create(string.Format("http://{0}{1}{2}", _hostname, (_port == 80 ? "" : ":" + _port), _path));
                route = new HttpResponse
                            {
                                Code = 200,
                                Message = "OK",
                                Body = new StreamReader(wr.GetResponse().GetResponseStream()).ReadToEnd()
                            };
            }

            return new Response(route.Code, route.Message, route.Body);
        }
    }
}