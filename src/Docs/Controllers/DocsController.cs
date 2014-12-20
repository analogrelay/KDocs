using System;
using System.Threading.Tasks;
using Docs.Services;
using Microsoft.AspNet.Mvc;

namespace Docs.Controllers
{
    public class DocsController : Controller
    {
        private readonly ContentService _contentService;

        public DocsController(ContentService contentService)
        {
            _contentService = contentService;
        }

        public async Task<IActionResult> Page(string path)
        {
            var content = await _contentService.GetContentAsync(path);

            if (content == null)
            {
                return HttpNotFound();
            }

            return View(content);
        }
    }
}