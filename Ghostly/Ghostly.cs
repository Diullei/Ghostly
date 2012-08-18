using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Ghostly
{
    public class Fs
    {

    }

    public class Ghostly
    {
        public class Process
        {
            private readonly string[] _args;
            private readonly IGhostlyJS _jsVm;

            public class Env
            {
                public string HOME = Environment.CurrentDirectory;
                public string TMPDIR = Path.GetTempPath();
                public string LANG = Environment.OSVersion.Platform.ToString();
            }

            public Process(string[] args, IGhostlyJS jsVm)
            {
                _natives_exports["events"] = Util.GetResource("events");
                _natives_exports["domain"] = Util.GetResource("domain");
                _natives_exports["buffer"] = Util.GetResource("buffer");
                _natives_exports["assert"] = Util.GetResource("assert");
                _natives_exports["util"] = Util.GetResource("util");
                _natives_exports["module"] = Util.GetResource("module");
                _natives_exports["path"] = Util.GetResource("path");
                _natives_exports["tty"] = Util.GetResource("tty");
                _natives_exports["url"] = Util.GetResource("url");
                _natives_exports["punycode"] = Util.GetResource("punycode");
                _natives_exports["querystring"] = Util.GetResource("querystring");

                _natives_exports["fs"] = "";
                _natives_exports["http"] = "";
                _natives_exports["https"] = "";
                _natives_exports["request"] = "";
                _natives_exports["http_parser"] = "exports.urlDecode = {}";

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

            //public Dictionary<string, object> global_exports = new Dictionary<string, object>();

            private Dictionary<string, object> _binding_cache = new Dictionary<string, object>();

            private Dictionary<string, object> _natives_exports = new Dictionary<string, object>();

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
                    return _natives_exports;
                }

                if (_natives_exports.ContainsKey(args))
                {
                    return _natives_exports[args];
                }

                throw new Exception("No such module: " + args);
            }

            private List<string> _requiredCache = new List<string>();

            public string getRealPath(string id, string dir)
            {
                if (id == "cssom")
                    return "cssom";

                string path = null;

                if (id.StartsWith(".\\") || id.StartsWith("./"))
                {
                    path = dir + id.Substring(1);
                }
                else if (id.StartsWith("..\\") || id.StartsWith("../"))
                {
                    path = dir + "\\" + id;
                }
                else
                {
                    var currentDirectory = Environment.CurrentDirectory;
                    path = Path.Combine(currentDirectory, id);
                }

                if (!File.Exists(path))
                {
                    path += ".js";
                    if (!File.Exists(path))
                    {
                        throw new Exception("Invalid require to inexistent file: " + path);
                    }
                }

                var fileInfo = new FileInfo(path);
                return fileInfo.FullName;
            }

            public object require(string id, string dir)
            {
                if (id == "cssom")
                    return new { source = "", filename = "cssim", dirname = "" };

                if (_natives_exports.ContainsKey(id))
                {
                    return _natives_exports[id];
                }

                //Console.WriteLine(string.Format("[id] {0}; [dir] {1}", id, dir));
                Console.WriteLine(string.Format("[id] {0}", id));

                string path = null;

                if (id.StartsWith(".\\") || id.StartsWith("./"))
                {
                    path = dir + id.Substring(1);
                }
                else if (id.StartsWith("..\\") || id.StartsWith("../"))
                {
                    path = dir + "\\" + id;
                }
                else
                {
                    var currentDirectory = Environment.CurrentDirectory;
                    path = Path.Combine(currentDirectory, id);
                }

                //path = ReducePath(path);

                if (!File.Exists(path))
                {
                    path += ".js";
                    if (!File.Exists(path))
                    {
                        throw new Exception("Invalid require to inexistent file: " + path);
                    }
                }

                var fileInfo = new FileInfo(path);
                path = fileInfo.FullName;

                //if (_requiredCache.Select(s => s.ToUpper()).Contains(path.ToUpper()))
                //    return true;

                _requiredCache.Add(path);

                return new { source = File.ReadAllText(path), filename = Path.GetFileName(path), dirname = Path.GetDirectoryName(path) };
            }

            public void console(string value)
            {
                Console.WriteLine(value);
            }
        }

        private static string ReducePath(string path)
        {
            var index = 0;
            var array = path.Split(new char[] {'\\', '/'}).ToList();
            while ((path.IndexOf("/../") != -1) && (path.IndexOf("\\..\\") != -1))
            {
                if (index >= array.Count)
                    continue;

                if(array[index] == "..")
                {
                    array.RemoveAt(index);
                    array.RemoveAt(index - 1);

                    index--;
                    index--;
                }
                else
                    index++;

                path = string.Join("\\", array);
            }

            return path;
        }

        private readonly IGhostlyJS _jsVm;

        public Ghostly(IGhostlyJS jsVm)
        {
            _jsVm = jsVm;
        }

        public void SetupProcessObject(IGhostlyJS jsVm, params string[] args)
        {
            var process = new Process(args, jsVm);
            _jsVm.SetParameter("process", process);
        }
    }
}