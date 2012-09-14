using System;

namespace Ghostly
{
    public class Browser
    {
        private readonly int _port;
        private PhantomjsWrapper _ph;

        public Browser() : this(-1) { }

        public Browser(int port)
        {
            _port = port == -1 ? 1234 : _port;
        }

        public void Visit(string url, Action action)
        {
            Visit(false, 15, url, action);
        }

        public void Visit(int timeOut, string url, Action action)
        {
            Visit(false, timeOut, url, action);
        }

        public void Visit(bool showPh, string url, Action action)
        {
            Visit(showPh, 15, url, action);
        }

        public void Visit(bool showPh, int timeOut, string url, Action action)
        {
            if (url.ToUpper().StartsWith("HTTPS:"))
                throw new Exception("Request to Https protocol is not working yet.");

            using (_ph = new PhantomjsWrapper())
            {
                _ph.Run(timeOut, showPh, "--web-security=no", 1234, url, action);
            }
        }

        public string Run(string code)
        {
            return _ph.Script(code);
        }
    }
}