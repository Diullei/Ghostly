// ===============================================================================
// Ghostly - .NET Headless Browser
// https://github.com/Diullei/Ghostly
//
// JsObjecProxy.cs
//
// This class is used to allow access to javascript objects form a .net dynamic 
// object
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
using System.Dynamic;
using System.Linq;

namespace Ghostly
{
    public class JsObjecProxy : DynamicObject
    {
        private readonly IGhostlyJS _ghostlyJS;
        private readonly string _ns;

        public JsObjecProxy(IGhostlyJS ghostlyJS, string @namespace)
        {
            _ghostlyJS = ghostlyJS;
            _ns = @namespace;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if ((bool)_ghostlyJS.Exec(string.Format("{0}.{1} == undefined", _ns, binder.Name)))
            {
                result = null;
                return false;
            }

            if ((bool)_ghostlyJS.Exec(string.Format("typeof {0}.{1} == 'object'", _ns, binder.Name)))
            {
                result = new JsObjecProxy(_ghostlyJS, string.Format("{0}.{1}", _ns, binder.Name));
                return true;
            }

            result = _ghostlyJS.Exec(string.Format("{0}.{1}", _ns, binder.Name));
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if ((bool)_ghostlyJS.Exec(string.Format("{0}.{1} == undefined", _ns, binder.Name)))
            {
                result = null;
                return false;
            }

            if ((bool)_ghostlyJS.Exec( string.Format("typeof {0}.{1}({2}) == 'object'", _ns, binder.Name, string.Join(", ", args.Select(ArgTypeToString).ToList())) ))
            {
                result = new JsObjecProxy(_ghostlyJS, string.Format("{0}.{1}({2})", _ns, binder.Name, string.Join(", ", args.Select(ArgTypeToString).ToList())));
                return true;
            }

            result = _ghostlyJS.Exec(string.Format("{0}.{1}('div_id')", _ns, binder.Name));
            return true;
        }

        private static string ArgTypeToString(object arg)
        {
            if (arg is String)
                return string.Format("'{0}'", arg.ToString().Replace("\'", "\\'"));
            return arg.ToString();
        }
    }
}
