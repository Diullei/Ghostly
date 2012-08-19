using System;

namespace Ghostly.Test
{
    public class TestSuite
    {
        public void Assert(bool condition)
        {
            if (!condition)
                throw new Exception("Invalid assert condition!");
        }
    }
}