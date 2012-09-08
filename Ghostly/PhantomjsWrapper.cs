using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Ghostly
{
    internal class PhantomjsWrapper : IDisposable
    {
        private string _exeName;
        private string _jsBootName;
        private Process _process;
        private HttpServer _server;

        public bool IsCallbackFinish { get; set; }

        public PhantomjsWrapper()
        {
            IsCallbackFinish = true;

            var guid = Guid.NewGuid().ToString();
            _exeName = guid + ".exe";
            _jsBootName = guid + ".js";
        }

        private void CreatePhantomJsExe()
        {
            Util.CreateTempFile("Ghostly.phantomjs.exe", _exeName);
        }

        private void CreateJsBoot(string url, int port)
        {
            using (var sw = new StreamWriter(_jsBootName))
            {
                sw.Write(Util.GetResource("/js_boot.js").Replace("###=URL=###", url).Replace("###=PORT=###", port.ToString()));
            }
        }

        public void Run(string args, int port, string url, Action acion)
        {
            _server = new HttpServer(acion, url, this);
            _server.Start(string.Format("http://localhost:{0}/", port));

            CreatePhantomJsExe();
            CreateJsBoot(url, port);

            _process = ProcessHelper.CreateAndStartProcess(_exeName, string.Format("{0} {1}", args, _jsBootName));
        }

        public string Script(string code)
        {
            var script = new Script(code);
            _server.AddScript(script);
            while (!script.Resolved)
            {
                Thread.Sleep(500);
            }

            return script.Result;
        }

        public void Dispose()
        {
            while (IsCallbackFinish)
            {
                Thread.Sleep(500);
            }

            _process.Kill();
            _process.Dispose();
            Thread.Sleep(100);
            _server.Stop();
            File.Delete(_exeName);
            File.Delete(_jsBootName);
        }
    }
}