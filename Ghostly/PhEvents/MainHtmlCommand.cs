using System.Net;

namespace Ghostly.PhEvents
{
    public class MainHtmlPhEvent : IPhEvent
    {
        public string Name
        {
            get { return "/main.html"; }
        }

        public string Exec(HttpListenerRequest request)
        {
            return Util.GetResource(request.Url.LocalPath);
        }
    }
}