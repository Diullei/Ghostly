using System;
using System.IO;
using Noesis.Javascript;

namespace Ghostly
{
    public class GhostlyJS : IGhostlyJS, IDisposable
    {
        private readonly JavascriptContext _context;

        public GhostlyJS()
        {
            _context = new JavascriptContext();
            new Ghostly(this).SetupProcessObject(this, new string[]{""});
            _context.Run(Util.GetResource("ghostly"));
            // move to browser
            _context.SetParameter("$___http___", new Http(new Route()));
        }

        public void SetParameter(string name, object value)
        {
            _context.SetParameter(name, value);
        }

        public object GetParameter(string name)
        {
            return _context.GetParameter(name);
        }

        public object Exec(string code)
        {
            return _context.Run(code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}