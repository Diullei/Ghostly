﻿using System;
using System.IO;
using Noesis.Javascript;

namespace Ghostly
{
    public class V8JsVM : IJsVM, IDisposable
    {
        private readonly JavascriptContext _context;

        public V8JsVM()
        {
            _context = new JavascriptContext();
            //new Ghostly(this).SetupProcessObject(this, new string[]{""});
            //_context.Run(Util.GetResource("ghostly"));
            _context.SetParameter("$___global___", new GlobalObjects(this));
            _context.Run(Util.GetResource("GlobalObjectsConfig"));
        }

        public void SetParameter(string name, object value)
        {
            _context.SetParameter(name, value);
        }

        public object GetParameter(string name)
        {
            return _context.GetParameter(name);
        }

        public object Exec(string code)
        {
            return _context.Run(code);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}