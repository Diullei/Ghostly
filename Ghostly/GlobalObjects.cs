using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Ghostly
{
    public class GlobalObjects
    {
        private readonly IGhostlyJS _jsvm;

        private Dictionary<string, object> _requireCache;

        public string Version
        {
            get
            {
                return "0.0.1";
            }
        }

        public string Home
        {
            get
            {
                return Environment.CurrentDirectory;
            }
        }

        public string Tmpdir
        {
            get
            {
                return Path.GetTempPath();
            }
        }

        public string CurrentPlatform
        {
            get
            {
                return Environment.OSVersion.Platform.ToString();
            }
        }

        public string Lang
        {
            get
            {
                return Thread.CurrentThread.CurrentCulture.Name;
            }
        }

        public GlobalObjects(IGhostlyJS jsvm)
        {
            _requireCache = new Dictionary<string, object>();
            _jsvm = jsvm;
        }

        public string GetSource(string file)
        {
            var currentDirectory = Environment.CurrentDirectory;
            var path = Path.Combine(currentDirectory, file);
            if (!File.Exists(path))
            {
                path += ".js";
                if (!File.Exists(path))
                {
                    throw new Exception("Invalid require to inexistent file: " + path);
                }
            }
            return File.ReadAllText(path);
        }

        public void ConsoleLog(object value)
        {
            Console.WriteLine(value);
        }
    }
}