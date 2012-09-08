using System;

namespace Ghostly
{
    public class Script
    {
        public Script(string code)
        {
            Code = code;
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; private set; }
        public string Code { get; set; }
        public bool Resolved { get; set; }
        public string Result { get; set; }
        public bool IsProcessing { get; set; }
    }
}