using System.Threading;

namespace Ghostly
{
    public class TimerObject
    {
        private readonly string _id;
        private readonly IGhostlyJS _ghostlyJS;
        private readonly bool _isTimeInterval;
        private readonly Timer _timer;
        private readonly TimerCallback _callback;
        private bool _wasDisposed;

        public TimerObject(string id, int interval, IGhostlyJS ghostlyJS, bool isTimeInterval)
        {
            _id = id;
            _ghostlyJS = ghostlyJS;
            _isTimeInterval = isTimeInterval;
            _callback = new TimerCallback(Tick);
            _timer = new Timer(_callback, null, 0, interval);
        }

        private void Tick(object stateInfo)
        {
            if (_wasDisposed)
                return;

            lock (this)
            {
                if (!_isTimeInterval)
                {
                    _timer.Dispose();
                    _wasDisposed = true;
                }
                _ghostlyJS.Exec(string.Format(
                    @"
if(typeof $__timerFunctionCallbackCollection__['{0}'] == 'function') {{
    $__timerFunctionCallbackCollection__['{0}']();
}} else if(typeof $__timerFunctionCallbackCollection__['{0}'] == 'string') {{
    eval($__timerFunctionCallbackCollection__['{0}'])();
}}
",
                    _id));
            }
        }
    }
}