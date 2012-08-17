using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ghostly
{
    public class Ghostly
    {
        public class Process
        {
            private readonly string[] _args;
            private readonly IJsVM _jsVm;

            public class Env
            {
                public string HOME = Environment.CurrentDirectory;
                public string TMPDIR = Path.GetTempPath();
                public string LANG = Environment.OSVersion.Platform.ToString();
            }

            public Process(string[] args, IJsVM jsVm)
            {
                _args = args;
                _jsVm = jsVm;
                _ghostlyScript = new GhostlyScript(_jsVm);
            }

            public string title
            {
                get { return Console.Title; }
                set { Console.Title = value; }
            }

            public string version
            {
                get { return Version.GhostlyVersion; }
            }

            private List<object> _moduleLoadList = new List<object>();

            public List<object> moduleLoadList
            {
                get { return _moduleLoadList; }
            }

            // VERSIONS

            // arch

            public string platform
            {
                get { return Environment.OSVersion.Platform.ToString(); }
            }

            public string[] argv
            {
                get { return _args; }
            }

            //execArgv

            private Env _env = new Env();

            public Env env
            {
                get { return _env; }
            }

            public int pid
            {
                get { return System.Diagnostics.Process.GetCurrentProcess().Id; }
            }

            //features

            //_eval

            //_print_eval = true

            //_forceRepl = true

            //noDeprecation = true

            //traceDeprecation = true

            public string execPath
            {
                get { return Environment.CurrentDirectory; }
            }

            public int debugPort = 0;

            //"_getActiveRequests", GetActiveRequests
            //"_getActiveHandles", GetActiveHandles
            //"_needTickCallback", NeedTickCallback
            //"reallyExit", Exit)
            //"abort", Abort
            //"chdir", Chdir
            // "cwd", Cwd

            public string cwd()
            {
                return "";
            }

            //"umask", Umask
            //"getuid", GetUid
            //"setuid", SetUid
            //"setgid", SetGid
            //"getgid", GetGid
            //"_kill", Kill
            //"_debugProcess", DebugProcess
            //"_debugPause", DebugPause
            //"_debugEnd", DebugEnd
            //"hrtime", Hrtime)
            //"dlopen", DLOpen)
            //"uptime", Uptime
            //"memoryUsage", MemoryUsage

            private GhostlyScript _ghostlyScript;

            private Dictionary<string, object> _binding_cache = new Dictionary<string, object>();

            public object binding(string args)
            {
                if (args == "evals")
                {
                    if (!_binding_cache.ContainsKey("evals"))
                        _binding_cache["evals"] = new Dictionary<string, object> { { "NodeScript", _ghostlyScript } };
                    return _binding_cache["evals"];
                }

                if (args == "buffer")
                {
                    if (!_binding_cache.ContainsKey("buffer"))
                        _binding_cache["buffer"] = new Dictionary<string, object> { { "SlowBuffer", new Dictionary<string, object>() } };
                    return _binding_cache["buffer"];
                }

                if (args == "constants")
                {
                    if (_binding_cache.ContainsKey(args))
                        return _binding_cache[args];

                    var exports = new Dictionary<string, object>();

                    _binding_cache[args] = exports;

                    return exports;
                }

                if (args == "natives")
                {
                    var exports = new Dictionary<string, object>();

                    exports["events"] = Util.GetResource("events");
                    exports["domain"] = Util.GetResource("domain");
                    exports["buffer"] = Util.GetResource("buffer");
                    exports["assert"] = Util.GetResource("assert");
                    exports["util"] = Util.GetResource("util");
                    exports["module"] = Util.GetResource("module");
                    exports["path"] = Util.GetResource("path");
                    exports["tty"] = Util.GetResource("tty");

                    return exports;
                }

                throw new Exception("No such module: " + args);
            }
        }

        private readonly IJsVM _jsVm;

        public Ghostly(IJsVM jsVm)
        {
            _jsVm = jsVm;
        }

        public void SetupProcessObject(IJsVM jsVm, params string[] args)
        {
            var process = new Process(args, jsVm);
            _jsVm.SetParameter("process", process);
        }
    }
}