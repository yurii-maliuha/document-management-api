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
        public IEnumerable<DocumentDTO> GetAll()
        {
            return _documentService.GetAll();
        }

        [HttpPost]
        public async Task<DocumentDTO> Create(IFormFile file)
        {
            var document = await _documentService.Create(file);
            return document;
        }

        [HttpPatch]
        public async Task<List<DocumentDTO>> Update([FromBody] List<DocumentDTO> documents)
        {
            var updatedDocuments = await _documentService.UpdateDocuments(documents);
            return updatedDocuments;
        }

        [HttpDelete]
        [Route("{name}/{id}")]
        public async Task Delete(string name, string id)
        {
            await _documentService.Delete(name, id);
        }
    }
}