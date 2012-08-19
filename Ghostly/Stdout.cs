using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghostly
{
    public class Stdout
    {
        //private Dictionary<string, ConsoleColor> _ansiCodes = new Dictionary<string, ConsoleColor>
        //                                                          {
        //                                                              {"\\e[0;30m", ConsoleColor.Black},
        //                                                              {"\\e[0;34m", ConsoleColor.Blue},
        //                                                              {"\\e[0;32m", ConsoleColor.Green},
        //                                                              {"\\e[0;36m", ConsoleColor.Cyan},
        //                                                              {"\\e[0;31m", ConsoleColor.Red},
        //                                                              //{"\\e[0;35m",	ConsoleColor.Purple},
        //                                                              //{"\\e[0;33m",	ConsoleColor.Brown},
        //                                                              {"\\e[0;37m", ConsoleColor.Gray},
        //                                                              {"\\e[1;30m", ConsoleColor.DarkGray},
        //                                                              //{"\\e[1;34m",	ConsoleColor.LightBlue},
        //                                                              //{"\\e[1;32m",	ConsoleColor.LightGreen},
        //                                                              //{"\\e[1;36m",	ConsoleColor.LightCyan},
        //                                                              //{"\\e[1;31m",	ConsoleColor.LightRed},
        //                                                              //{"\\e[1;35m",	ConsoleColor.LightPurple},
        //                                                              {"\\e[1;33m", ConsoleColor.Yellow},
        //                                                              {"\\e[1;37m", ConsoleColor.White}
        //                                                          };

        public void Write(string value)
        {
            /*while (Regex.IsMatch(value, "\\\\e\\[\\w;\\w\\wm"))
            {
                var groups = Regex.Match(value, "\\\\e\\[\\w;\\w\\wm").Groups;
                foreach(Group g in groups)
                {
                    Console.Write(value.Substring(0, g.Index));
                    Console.ForegroundColor = _ansiCodes[value.Substring(g.Index, g.Index)];
                    value = value.Substring(g.Index *2);
                }
            }*/
            Console.Write(value);
        }
    }
}