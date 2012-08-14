using System;
using org.mozilla.javascript;

namespace Ghostly
{
    public class Browser
    {
        private Action _callback;
        private string _url;

        public Context Context { get; private set; }

        public ScriptableObject Scope { get; private set; }

        private void InitRhino()
        {
            Context = ContextFactory.getGlobal().enterContext();
            Context.setOptimizationLevel(-1);
            Context.setLanguageVersion(Context.VERSION_DEFAULT);
            Scope = Context.initStandardObjects();
        }

        private void WrapBrowser()
        {
            java.lang.Class bClass = typeof(Browser);
            java.lang.reflect.Member method = bClass.getMethod("StaticCallback", typeof(object));
            Scriptable function = new FunctionObject("StaticCallback", method, Scope);
            Scope.put("StaticCallback", Scope, function);
            ScriptableObject.putProperty(Scope, "browser", this);
        }

        private void InitEnvjs()
        {
            var envjs = System.IO.File.ReadAllText("env.rhino.js");
            Context.evaluateString(Scope, "function print(message) { java.lang.System.out.println(message); }", "print", 1, null);
            Context.evaluateString(Scope, envjs, "env.rhino.js", 1, null);
        }

        private void Init()
        {
            InitRhino();
            WrapBrowser();
            InitEnvjs();
        }

        private void LoadUrl()
        {
            Exec(string.Format("window.location='{0}'", _url));
            Context.evaluateString(Scope, "(function(){ StaticCallback(browser) })()", "<cmd>", 1, null);
        }

        public void Visit(string url, BrowserOptions options, Action callback)
        {
            _callback = callback;
            _url = url;
            Init();
            LoadUrl();
        }

        public static string StaticCallback(object browser)
        {
            if (browser is Browser)
                ((Browser)browser).Callback();
            else
                throw new Exception("Wrong class");

            return "";
        }

        public void Callback()
        {
            _callback.Invoke();
        }

        private object Exec(string script)
        {
            return Context.evaluateString(Scope, script, "_" + Guid.NewGuid().ToString("N"), 1, null);
        }

        public T ExecScript<T>(string script)
        {
            if (typeof(T) == typeof(String))
            {
                return (T)(object)Context.toString(Exec(script));
            }

            if (typeof(T) == typeof(Int32))
            {
                return (T)(object)Context.toNumber(Exec(script));
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)Context.toBoolean(Exec(script));
            }

            throw new Exception("Invalid return type");
        }
    }
}