using System;
using System.IO;
using System.Reflection;

namespace Ghostly
{
    public class Util
    {
        public static void CopyStream(Stream input, Stream output)
        {
            // Insert null checking here for production
            var buffer = new byte[8192];

            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }

        public static string GetResource(string path)
        {
            try
            {
                return new StreamReader(
                    Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Ghostly" + path.Replace("/", "."))).ReadToEnd();
            }
            catch (Exception)
            {
                return string.Format("<html><body>Resource not found: {0}</body></html>", path);
            }
        }

        public static void CreateTempFile(string resource, string name)
        {
            using (var input = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                using (var output = File.Create(name))
                {
                    CopyStream(input, output);
                }
            }
        }
    }
}