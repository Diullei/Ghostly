using System;
using System.Collections;
using System.Linq;
using System.Net;

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
                System.IO.Stream body = request.InputStream;
                System.Text.Encoding encoding = request.ContentEncoding;
                System.IO.StreamReader reader = new System.IO.StreamReader(body, encoding);
                //if (request.ContentType.ToLower() == "application/x-www-form-urlencoded")
                {
                    string s = reader.ReadToEnd();
                    string[] pairs = s.Split('&');
                    for (int x = 0; x < pairs.Length; x++)
                    {
                        string[] item = pairs[x].Split('=');
                        formVars.Add(item[0], System.Web.HttpUtility.UrlDecode(item[1]));
                    }
                }
                body.Close();
                reader.Close();
            }
            return formVars;
        }

        public string Exec(HttpListenerRequest request)
        {
            try
            {
                //var vars = GetFormValues(request);

                var script = _scripts.Where(s => s.Id == request.QueryString["id"]).First();
                script.Result = request.QueryString["result"];
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