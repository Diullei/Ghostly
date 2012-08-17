using System;

namespace Ghostly
{
    public class GhostlyScript
    {
        private readonly IJsVM _jsVm;

        public GhostlyScript(IJsVM jsVm)
        {
            _jsVm = jsVm;
        }

        public object runInThisContext(string source, string fileName, bool flg)
        {
            return null;
        }
    }
}