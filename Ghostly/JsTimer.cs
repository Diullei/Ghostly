namespace Ghostly
{
    public class JsTimer
    {
        private readonly IGhostlyJS _ghostlyJS;

        public JsTimer(IGhostlyJS ghostlyJS)
        {
            _ghostlyJS = ghostlyJS;
        }

        public void Register(string id, int interval, bool isTimeInterval)
        {
            new TimerObject(id, interval, _ghostlyJS, isTimeInterval);
        }
    }
}