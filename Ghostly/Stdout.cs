using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghostly
{
    public class Stdout
    {
        private Dictionary<string, ConsoleColor> _ansiCodes = new Dictionary<string, ConsoleColor>
            {
                {Char.ConvertFromUtf32(27) + "[0m", ConsoleColor.Gray},
                {Char.ConvertFromUtf32(27) + "[30m", ConsoleColor.Black},
                {Char.ConvertFromUtf32(27) + "[34m", ConsoleColor.Blue},
                {Char.ConvertFromUtf32(27) + "[32m", ConsoleColor.Green},
                {Char.ConvertFromUtf32(27) + "[36m", ConsoleColor.Cyan},
                {Char.ConvertFromUtf32(27) + "[31m", ConsoleColor.Red},
                {Char.ConvertFromUtf32(27) + "[35m", ConsoleColor.Magenta},
                {Char.ConvertFromUtf32(27) + "[33m", ConsoleColor.Yellow},
                {Char.ConvertFromUtf32(27) + "[37m", ConsoleColor.White}
            };

        public void Write(string value)
        {
            var currentColor = Console.ForegroundColor;
            var queue = new Queue<KeyValuePair<ConsoleColor, string>>();

            while (Regex.IsMatch(value, Char.ConvertFromUtf32(27) + "\\[\\d{1,2}m"))
            {
                var groups = Regex.Match(value, Char.ConvertFromUtf32(27) + "\\[\\d{1,2}m").Groups;
                foreach(Group g in groups)
                {
                    queue.Enqueue(new KeyValuePair<ConsoleColor, string>(currentColor, value.Substring(0, g.Index)));
                    currentColor = _ansiCodes[g.Value];
                    value = value.Substring(g.Index + g.Value.Length);
                }
            }
            queue.Enqueue(new KeyValuePair<ConsoleColor, string>(currentColor, value.Substring(0)));

            while (queue.Count > 0)
            {
                var s = queue.Dequeue();
                Console.ForegroundColor = s.Key;
                Console.Write(s.Value);
            }
        }

        public string Readln()
        {
            return Console.ReadLine();
        }
    }
}