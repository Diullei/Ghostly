using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ghostly
{
    public class ProcessHelper
    {
        public static Process CreateAndStartProcess(string target, string args)
        {
            var process = new Process
                              {
                                  StartInfo =
                                      {
                                          UseShellExecute = false,
                                          CreateNoWindow = true,
                                          WindowStyle = ProcessWindowStyle.Hidden,
                                          FileName = target,
                                          Arguments = args,
                                          RedirectStandardError = true,
                                          RedirectStandardOutput = true
                                      }
                              };

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            return process;
        }
    }
}
