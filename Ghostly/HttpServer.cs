using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Ghostly.PhEvents;

namespace Ghostly
{
    internal class HttpServer
    {
        readonly PhEventSet _commands;
        readonly HttpListener _listener;
        readonly ScriptSet _scripts;

        public HttpServer(Action action, string url, PhantomjsWrapper wrapper)
        {
            _listener = new HttpListener();
            _scripts = new ScriptSet();

            _commands = new PhEventSet 
            { 
                new OnLoadPhEvent(action, wrapper), 
                new MainHtmlPhEvent(),
                new JQueryPhEvent(),
                new ScriptPhEvent(_scripts),
                new CallbackPhEvent(_scripts)
            };
        }

        public void AddScript(Script script)
        {
            _scripts.Add(script);
        }

        public void Start(string url)
        {
            _listener.Prefixes.Add(url);
            _listener.Start();
            Console.WriteLine("Listening, hit enter to stop");
            _listener.BeginGetContext(new AsyncCallback(GetContextCallback), null);
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public void GetContextCallback(IAsyncResult result)
        {
            try
            {
                var context = _listener.EndGetContext(result);
                var request = context.Request;
                var response = context.Response;

                var responseString = "";
                var command = _commands.Get(request.Url.LocalPath);
                if (command != null)
                {
                    responseString = command.Exec(request);
                }

                var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                response.ContentLength64 = buffer.Length;

                using (var outputStream = response.OutputStream)
                {
                    outputStream.Write(buffer, 0, buffer.Length);
                }
                _listener.BeginGetContext(new AsyncCallback(GetContextCallback), null);
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode != 995 && ex.ErrorCode != 1229)
                    throw;
            }
        }
    }
}
