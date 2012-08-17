namespace Ghostly
{
    public interface IJsVM
    {
        void SetParameter(string name, object value);
        object GetParameter(string name);
        object Exec(string code);
    }
}