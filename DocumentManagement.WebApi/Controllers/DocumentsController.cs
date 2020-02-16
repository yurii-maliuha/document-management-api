using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Common.Models;
using DocumentManagement.WebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "files1", "files2" };
        }

        [HttpPost]
        public async Task<DocumentDTO> Create(IFormFile file)
        {
            var document = await _documentService.Create(file);
            return document;
        }
    }
}