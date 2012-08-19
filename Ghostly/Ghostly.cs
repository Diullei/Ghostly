using System.Collections;
using System.Linq;

namespace Ghostly
{
    public class Fs
    {

    }

    public class Ghostly
    {
        private readonly IGhostlyJS _jsVm;

        public Ghostly(IGhostlyJS jsVm)
        {
            _jsVm = jsVm;
        }

        public void SetupProcessObject(IGhostlyJS jsVm, params string[] args)
        {
            var process = new Process(args, jsVm);
            _jsVm.SetParameter("process", process);
            var stdout = new Stdout();
            _jsVm.SetParameter("$__stdout__", stdout);
        }
    }
}