using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ghostly.PhEvents
{
    public class CallbackPhEvent : IPhEvent
    {
        private ScriptSet _scripts;

        public CallbackPhEvent(ScriptSet scripts)
        {
            _scripts = scripts;
        }

        public string Name
        {
            get { return "/callback"; }
        }

        private Hashtable GetFormValues(HttpListenerRequest request)
        {
            Hashtable formVars = new Hashtable();

            //add request data at bottom of page
            if (request.HasEntityBody)
            {
                var s = new StreamReader(request.InputStream).ReadToEnd();
                //string s = reader.ReadToEnd();
                string[] pairs = s.Split('&');
                for (int x = 0; x < pairs.Length; x++)
                {
                    string[] item = pairs[x].Split('=');
                    formVars.Add(item[0], System.Web.HttpUtility.UrlDecode(item[1]));
                }
            }
            return formVars;
        }

        public string Exec(HttpListenerRequest request)
        {
            try
            {
                var vars = GetFormValues(request);

                var script = _scripts.Where(s => s.Id == (string)vars["id"]).First();
                script.Result = (string)vars["result"];
                script.Resolved = true;
                _scripts.Remove(script);
            }
            catch (Exception)
            {
            }

            return "";
        }
    }
}