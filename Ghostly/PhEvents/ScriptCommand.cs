using System;
using System.Linq;
using System.Net;

namespace Ghostly.PhEvents
{
    public class ScriptPhEvent : IPhEvent
    {
        private ScriptSet _scripts;

        public ScriptPhEvent(ScriptSet scripts)
        {
            _scripts = scripts;
        }

        public string Name
        {
            get { return "/com"; }
        }

        public string Exec(HttpListenerRequest request)
        {
            try
            {
                var script = _scripts.Where(s => !s.IsProcessing).First();
                script.IsProcessing = true;
                return string.Format("{{ id: '{0}', script: '{1}' }}", script.Id, script.Code.Replace("'", "\'"));
            }
            catch (Exception)
            {
            }

            return "";
        }
    }
}