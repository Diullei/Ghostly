using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Ghostly
{
    public class DomProxy : DynamicObject
    {
        private readonly IJsVM _jsVm;
        private readonly string _ns;

        public DomProxy(IJsVM jsVm, string @namespace)
        {
            _jsVm = jsVm;
            _ns = @namespace;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if ((bool)_jsVm.Exec(string.Format("{0}.{1} == undefined", _ns, binder.Name)))
            {
                result = null;
                return false;
            }

            if ((bool)_jsVm.Exec(string.Format("typeof {0}.{1} == 'object'", _ns, binder.Name)))
            {
                result = new DomProxy(_jsVm, string.Format("{0}.{1}", _ns, binder.Name));
                return true;
            }

            result = _jsVm.Exec(string.Format("{0}.{1}", _ns, binder.Name));
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if ((bool)_jsVm.Exec(string.Format("{0}.{1} == undefined", _ns, binder.Name)))
            {
                result = null;
                return false;
            }

            if ((bool)_jsVm.Exec( string.Format("typeof {0}.{1}({2}) == 'object'", _ns, binder.Name, string.Join(", ", args.Select(ArgTypeToString).ToList())) ))
            {
                result = new DomProxy(_jsVm, string.Format("{0}.{1}({2})", _ns, binder.Name, string.Join(", ", args.Select(ArgTypeToString).ToList())));
                return true;
            }

            result = _jsVm.Exec(string.Format("{0}.{1}('div_id')", _ns, binder.Name));
            return true;
        }

        private string ArgTypeToString(object arg)
        {
            if (arg is String)
                return string.Format("'{0}'", arg.ToString().Replace("\'", "\\'"));
            return arg.ToString();
        }
    }
}
