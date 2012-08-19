namespace Ghostly
{
    public interface IGhostlyJS
    {
        void SetParameter(string name, object value);
        object GetParameter(string name);
        object Exec(string code);
    }
}