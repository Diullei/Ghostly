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

            /**
             * this will now load and run all external javascript, 
             * emulating browser behavior
             */
            _jsvm.Exec(@"
Envjs({
    scriptTypes : {
        '': true, //inline and anonymous
        'text/javascript': true,
        'text/envjs': true
    },
    beforeScriptLoad:{
        'urchin': function(scriptNode){
            console.log('scriptNode.src: ' + scriptNode.src);
            scriptNode.src = '';
        }
    }
});

Envjs.loadInlineScript = function(scriptNode){
   //load and execute the javascript from the node
   console.log('(1) scriptNode: ' + scriptNode.src);
   return true;//false if error
}

Envjs.loadLocalScript = function(scriptNode){
   //load and execute the javascript from the url
   console.log('(2) scriptNode: ' + scriptNode.innerHTML);
   return true;//false if error
}");

            _jsvm.Exec("require('envjs/window');");
        }
    }
}