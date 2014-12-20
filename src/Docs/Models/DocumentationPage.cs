using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Rendering;

namespace Docs.Models
{
    /// <summary>
    /// Represents a single page of documentation
    /// </summary>
    public class DocumentationPage
    {
        public string FileName { get; }
        public FrontMatter FrontMatter { get; }
        public HtmlString Content { get; }

        public IEnumerable<Heading> Headings { get; }

        public string Title { get { return FrontMatter.GetString("title", GetDefaultTitle()); } }

        public DocumentationPage(string fileName, FrontMatter frontMatter, HtmlString content)
        {
            FileName = fileName;
            FrontMatter = frontMatter;
            Content = content;
        }

        private string GetDefaultTitle()
        {
            return Path.GetFileNameWithoutExtension(FileName);
        }
    }
}