using System;

namespace Ghostly
{
    public class TestSuite
    {
        public void Assert(bool condition)
        {
            if (!condition)
                throw new Exception("Invalid assert condition!");
        }
    }

    public class Browser
    {
        private Action _callback;
        private string _url;

        private V8JsVM Vm { get; set; }

        public dynamic Window
        {
            get { return new DomProxy(Vm, "window"); }
        }

        public Route Route { get; private set; }

        public TestSuite Test { get; set; }

        public Browser()
        {
            Test = new TestSuite();
            Route = new Route();
            Vm = new V8JsVM();
            //Vm.SetParameter("$___http___", new Http(Route));
        }

        private void Init()
        {
            var envJs = new EnvJs(Vm);
            envJs.Init();
            Vm.SetParameter("browser", this);
        }

        private void LoadUrl()
        {
            Vm.Exec(string.Format("window = new Window(); window.location='{0}'", _url));
            Vm.Exec("(function(){ browser.Callback(); })()");
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

        public T ExecScript<T>(string script)
        {
            if (typeof(T) == typeof(String))
            {
                return (T)(object)Vm.Exec(script);
            }

            if (typeof(T) == typeof(Int32))
            {
                return (T)(object)Vm.Exec(script);
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)Vm.Exec(script);
            }

            throw new Exception("Invalid return type");
        }
    }
}