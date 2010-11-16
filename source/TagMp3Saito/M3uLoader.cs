using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TagMp3Saito
{
    public static class M3uPathExtractor
    {
        public static string[] ExtractPaths(string m3uPath)
        {
            var pathsExtracted = new List<string>();

            var sr = new StreamReader(m3uPath, Encoding.Default);
            string content = sr.ReadToEnd();
            sr.Close();

            var regexPaths = new Regex(@"^([^#][^\n\r]*)", RegexOptions.Compiled | RegexOptions.Multiline);
            foreach (Match m in regexPaths.Matches(content))
                pathsExtracted.Add(m.Groups[1].Value);

            return pathsExtracted.ToArray();
        }
    }
}