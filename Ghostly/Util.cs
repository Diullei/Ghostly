using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Ghostly
{
    public class Util
    {
        public static string GetResource(string id)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (string.IsNullOrWhiteSpace(id))
                return null;

            if (!id.EndsWith(".js"))
                id += ".js";

            try
            {
                return new StreamReader(assembly.GetManifestResourceStream("Ghostly." + id)).ReadToEnd();
            }
            catch (Exception)
            {
                try
                {
                    return new StreamReader(assembly.GetManifestResourceStream("Ghostly.js." + id)).ReadToEnd();
                }
                catch (Exception)
                {
                    if (File.Exists(id))
                    {
                        return File.ReadAllText(id);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
