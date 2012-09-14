using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ghostly
{
    public class ProcessHelper
    {
        public static Process CreateAndStartProcess(bool showPh, string target, string args)
        {
            var process = new Process
                              {
                                  StartInfo =
                                      {
                                          UseShellExecute = showPh,
                                          CreateNoWindow = !showPh,
                                          WindowStyle = showPh ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
                                          FileName = target,
                                          Arguments = args,
                                          RedirectStandardError = !showPh,
                                          RedirectStandardOutput = !showPh
                                      }
                              };

            process.StartInfo.UseShellExecute = showPh;
            process.StartInfo.CreateNoWindow = !showPh;
            process.Start();

            return process;
        }
    }
}