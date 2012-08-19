// ===============================================================================
// Ghostly - .NET Headless Browser
// https://github.com/Diullei/Ghostly
//
// Browser.cs
//
// The browser object
// ===============================================================================
// SINCE VERSION: 0.1.0
// ===============================================================================
// Copyright (c) 2012 by Diullei Gomes
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to permit
// persons to whom the Software is furnished to do so, subject to the
// following conditions:
//
// The above copyright notice and this permission notice shall be included
// in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
// NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
// USE OR OTHER DEALINGS IN THE SOFTWARE.
// ===============================================================================
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Ghostly.Test;

namespace Ghostly
{
    public class Browser
    {
        private Action<string, dynamic> _callback;
        
        private string _url;

        private bool _wasInitialized = false;

        private GhostlyJS GhostlyJS { get; set; }

        public Route Route { get; private set; }

        public TestSuite Test { get; set; }

        public dynamic Js
        {
            get { return GhostlyJS.Js; }
        }

        public int StatusCode { get; private set; }

        public Browser()
        {
            Test = new TestSuite();
            Route = new Route();
        }

        public void Init()
        {
            if (_wasInitialized)
                return;

            GhostlyJS = new GhostlyJS(new string[] { });
            GhostlyJS.SetParameter("$___http___", new Http(new Route()));
            GhostlyJS.Exec("global.jsdom = require('js/jsdom/jsdom');");
            GhostlyJS.SetParameter("$__browser__", this);
            _wasInitialized = true;
        }

        private void LoadUrl()
        {
            GhostlyJS.Exec(
                string.Format(
                    "global.jsdom.env('{0}', function(errors, window) {{ global.window = window; $__browser__.Callback(errors); }})", _url));
        }

        public void Visit(string url, BrowserOptions options, Action<string, dynamic> callback)
        {
            if (Route.Interceptors.ContainsKey(url))
            {
                url = Route.Interceptors
                    .Where(
                        i =>
                        i.Key.ToUpper() == url.ToUpper())
                        .First().Value.Invoke().Body;
            }

            _callback = callback;
            _url = Regex.Replace(url, "\\r\\n", "\\\n");
            Init();
            LoadUrl();
        }

        public void Callback(string errors)
        {
            _callback.Invoke(errors, GhostlyJS.Js.window);
        }

        public void ExecScript(string script)
        {
            GhostlyJS.Exec(script);
        }

        public T ExecScript<T>(string script)
        {
            if (typeof(T) == typeof(String))
            {
                return (T)(object)GhostlyJS.Exec(script);
            }

            if (typeof(T) == typeof(Int32))
            {
                return (T)(object)GhostlyJS.Exec(script);
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)GhostlyJS.Exec(script);
            }

            throw new Exception("Invalid return type");
        }
    }
}