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
            using (_ph = new PhantomjsWrapper())
            {
                _ph.Run("--web-security=no", 1234, url, action);
            }
        }

        public string Run(string code)
        {
            return _ph.Script(code);
        }
    }
}