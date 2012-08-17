using System.IO;

namespace Ghostly
{
    public class EnvJs
    {
        private readonly IJsVM _jsvm;

        public EnvJs(IJsVM jsvm)
        {
            _jsvm = jsvm;
        }

        public void Init()
        {
            var endJsConfigCode = File.ReadAllText("js\\EnvJsConfig.js");
            _jsvm.Exec(endJsConfigCode);
            _jsvm.Exec("require('envjs/window');");
        }
    }
}