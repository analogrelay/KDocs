using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Docs.Models;
using Microsoft.AspNet.FileSystems;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace Docs.Services
{
    public abstract class ContentRenderer
    {
        public abstract IEnumerable<string> SupportedExtensions { get; }

        public virtual async Task<DocumentationPage> Render(IFileInfo file)
        {
            // Load the file as a string.
            FrontMatter frontMatter = null;
            StringBuilder inputBuilder = new StringBuilder();
            using (var strm = file.CreateReadStream())
            {
                using (var reader = new StreamReader(strm))
                {
                    // Try to read front matter
                    var result = await FrontMatter.TryReadFrontMatter(reader);
                    frontMatter = result.FrontMatter;
                    if (result.NonFrontMatterContent != null)
                    {
                        inputBuilder.Append(result.NonFrontMatterContent);
                    }

                    inputBuilder.Append(await reader.ReadToEndAsync());
                }
            }

            return new DocumentationPage(
                file.Name,
                frontMatter,
                await RenderContent(inputBuilder.ToString()));
        }

        public virtual Task<HtmlString> RenderContent(string input)
        {
            throw new NotImplementedException("One of Render(IFileInfo) or Render(string) must be implemented!");
        }
    }
}