using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Docs.Models;
using HtmlAgilityPack;
using Microsoft.AspNet.FileSystems;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace Docs.Services
{
    public class HtmlContentRenderer : ContentRenderer
    {
        private static readonly Regex HeadingTagMatch = new Regex(@"h\d", RegexOptions.IgnoreCase);
        public override IEnumerable<string> SupportedExtensions
        {
            get
            {
                yield return "html";
                yield return "htm";
            }
        }

        public override Task<HtmlString> RenderContent(string input)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(input);

            // Find headings (borrowed from https://github.com/NuGet/NuGetDocs/blob/master/Docs.Core/MarkdownEngine/MarkdownWebPage.cs)
            var allNodes = doc.DocumentNode.Descendants();
            var allHeadingNodes = allNodes.Where(node => HeadingTagMatch.IsMatch(node.Name)).ToList();

            var headings = new List<HeadingInfo>();
            foreach (var heading in allHeadingNodes)
            {
                var id = heading.InnerHtml.Replace(" ", "_");

                id = WebUtility.HtmlEncode(WebUtility.UrlEncode(id)); // TODO: What encoding should happen here?

                // The Regex ensured that the tag is definitely "hN" where N is an Integer between 0 and 9.
                headings.Add(new HeadingInfo(id, heading.InnerHtml, int.Parse(heading.Name.Remove(0, 1))));
            }

            // Now process the infos into a tree
            var headings = TreeifyHeadings(headings, currentLevel: 0);

            // Not really any rendering to be done!
            return Task.FromResult(new HtmlString(input));
        }

        private IEnumerable<Heading> TreeifyHeadings(List<HeadingInfo> headings, int currentLevel)
        {
            foreach (var heading in headings)
            {
                if(heading
            }
        }

        private class HeadingInfo
        {
            public string Id { get; }
            public string Name { get; }
            public int Level { get; }

            public HeadingInfo(string id, string name, int level)
            {
                Id = id;
                Name = name;
                Level = level;
            }
        }
    }
}