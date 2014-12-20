using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Docs.Services;

namespace Docs.Models
{
    public class FrontMatter
    {
        public static readonly string Delimiter = "---";

        public IDictionary<string, object> Values { get; private set; }

        public FrontMatter(IDictionary<string, object> values)
        {
            Values = values;
        }

        public string GetString(string key, string defaultValue)
        {
            object value;
            if (Values.TryGetValue(key, out value))
            {
                return (string)value;
            }
            else
            {
                return defaultValue;
            }
        }

        internal static async Task<FrontMatterParseResult> TryReadFrontMatter(TextReader input)
        {
            // Create a string builder to capture any content outside the front matter that we have to read to find the front matter
            StringBuilder nonMatterBuilder = new StringBuilder();

            // Read lines until the first non-whitespace one
            string line = null;
            while (string.IsNullOrWhiteSpace(line = await input.ReadLineAsync()))
            {
                nonMatterBuilder.AppendLine(line);
            }

            // Ok, now we have the first non-whitespace line. Check for front matter
            FrontMatter frontMatter = null;
            if (string.Equals(line.Trim(), Delimiter, StringComparison.Ordinal))
            {
                // It is! Read until the end of the front matter
                StringBuilder frontMatterContent = new StringBuilder();
                while (!string.Equals(line = await input.ReadLineAsync(), Delimiter, StringComparison.Ordinal))
                {
                    frontMatterContent.AppendLine(line);
                }

                // Input is positioned at the first non-front matter content, so we're done!
                frontMatter = new FrontMatter(BadYamlParser.Parse(frontMatterContent.ToString()));
            }
            else
            {
                // Not front-matter, put the line in the content (along with a terminator)
                nonMatterBuilder.AppendLine(line);
            }

            return new FrontMatterParseResult(
                frontMatter,
                nonMatterBuilder.ToString());
        }
    }

    public class FrontMatterParseResult
    {
        public FrontMatter FrontMatter { get; }
        public string NonFrontMatterContent { get; }

        public FrontMatterParseResult(FrontMatter frontMatter, string nonFrontMatterContent)
        {
            FrontMatter = frontMatter;
            NonFrontMatterContent = nonFrontMatterContent;
        }
    }
}