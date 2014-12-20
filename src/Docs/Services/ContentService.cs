using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Docs.Models;
using Microsoft.AspNet.FileSystems;

namespace Docs.Services
{
    /// <summary>
    /// Represents the default content service
    /// </summary>
    public class ContentService
    {
        private readonly IEnumerable<ContentRenderer> _renderers;
        private readonly IFileSystem _rootFileSystem;

        public ContentService(IFileSystem rootFileSystem, params ContentRenderer[] renderers)
            : this(rootFileSystem, (IEnumerable<ContentRenderer>)renderers)
        { }

        public ContentService(IFileSystem rootFileSystem, IEnumerable<ContentRenderer> renderers)
        {
            _rootFileSystem = rootFileSystem;
            _renderers = renderers;
        }

        public virtual async Task<DocumentationPage> GetContentAsync(string path)
        {
            // Try to find a file supported by a renderer
            path = path ?? String.Empty;
            foreach (var renderer in _renderers)
            {
                foreach (var ext in renderer.SupportedExtensions)
                {
                    string indexPath = Path.Combine(path, "index." + ext);
                    string filePath = path + "." + ext.TrimStart('.');
                    IFileInfo file;
                    if (_rootFileSystem.TryGetFileInfo(indexPath, out file) || _rootFileSystem.TryGetFileInfo(filePath, out file))
                    {
                        return await renderer.Render(file);
                    }
                }
            }

            // No results
            return null;
        }
    }
}