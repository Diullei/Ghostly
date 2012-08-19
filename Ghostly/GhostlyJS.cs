// ===============================================================================
// Ghostly - .NET Headless Browser
// https://github.com/Diullei/Ghostly
//
// GhostlyJS.cs
//
// This class is used to wrap V8 engine using IGhostlyJS interface
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
using Noesis.Javascript;

namespace Ghostly
{
    public class GhostlyJS : IGhostlyJS, IDisposable
    {
        private readonly JavascriptContext _context;

        public GhostlyJS(string[] args)
        {
            _context = new JavascriptContext();
            new Ghostly(this).Initialize(this, args);
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