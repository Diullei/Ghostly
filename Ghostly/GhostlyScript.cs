using System;

namespace Ghostly
{
    public class GhostlyScript
    {
        private readonly IGhostlyJS _jsVm;

        public GhostlyScript(IGhostlyJS jsVm)
        {
            _jsVm = jsVm;
        }

        public object runInThisContext(string source, string fileName, bool flg)
        {
            return null;
        }
    }
}