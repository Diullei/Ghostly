using System.Net;

namespace Ghostly.PhEvents
{
    public interface IPhEvent
    {
        string Name { get; }
        string Exec(HttpListenerRequest request);
    }
}