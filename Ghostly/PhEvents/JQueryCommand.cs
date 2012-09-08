using System.Net;

namespace Ghostly.PhEvents
{
    public class JQueryPhEvent : IPhEvent
    {
        public string Name
        {
            get { return "/jquery.js"; }
        }

        public string Exec(HttpListenerRequest request)
        {
            return Util.GetResource(request.Url.LocalPath);
        }
    }
}