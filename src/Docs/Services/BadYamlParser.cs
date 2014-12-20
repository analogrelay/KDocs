using System;
using System.Collections.Generic;
using System.IO;

namespace Docs.Services
{
    /// <summary>
    /// A terrible, horrible, no good, very bad Yaml Parser
    /// </summary>
    internal static class BadYamlParser
    {
        public static IDictionary<string, object> Parse(string yaml)
        {
            var results = new Dictionary<string, object>(StringComparer.Ordinal); // Yaml is case-sensitive
            using (var reader = new StringReader(yaml))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    // Read to first colon
                    int colonIdx = line.IndexOf(':');
                    if (colonIdx >= 0)
                    {
                        string key = line.Substring(0, colonIdx);
                        string value = line.Substring(colonIdx + 1);
                        results[key] = value;
                    }
                    // else: No colon, ignore this line
                }
            }
            return results;
        }
    }
}