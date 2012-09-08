using System.Collections.Generic;

namespace Ghostly.PhEvents
{
    public class PhEventSet : List<IPhEvent>
    {
        public IPhEvent Get(string name)
        {
            foreach (var command in this)
            {
                if (command.Name.ToUpper() == name.ToUpper())
                {
                    return command;
                }
            }

            return null;
        }
    }
}