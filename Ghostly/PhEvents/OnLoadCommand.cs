using System;
using System.Net;
using System.Threading;

namespace Ghostly.PhEvents
{
    internal class OnLoadPhEvent : IPhEvent
    {
        private Action _action;
        private readonly PhantomjsWrapper _wrapper;

        public OnLoadPhEvent(Action action, PhantomjsWrapper wrapper)
        {
            _action = action;
            _wrapper = wrapper;
        }

        public string Name
        {
            get { return "/onload"; }
        }

        private void ExecuteInBiggerStackThread(Action action)
        {
            var operation = new ParameterizedThreadStart(obj =>
                                                             {
                                                                 action();
                                                                 _wrapper.IsCallbackFinish = false;
                                                             });
            Thread bigStackThread = new Thread(operation, 1024 * 1024);

            bigStackThread.Start();
            //bigStackThread.Join();
        }

        public string Exec(HttpListenerRequest request)
        {
            ExecuteInBiggerStackThread(_action);
            //_action.Invoke();
            return "";
        }
    }
}