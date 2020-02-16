using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentManagement.Common.Models;
using DocumentManagement.DAL.Azure;
using DocumentManagement.DAL.Services;
using DocumentManagement.Models;
using DocumentManagement.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DocumentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IDocumentService _documentService;

        public DocumentsController(ILogger<DocumentsController> logger,
            IDocumentService documentService)
        {
            _logger = logger;
            _documentService = documentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DocumentDTO>> GetAll()
        {
            var documents = _documentService.GetAll();
            return Ok(documents);
        }

        [HttpPost]
        public async Task<ActionResult<DocumentDTO>> Create(IFormFile file)
        {

            if (file.Length / 1000000 >= 5)
            {
                return BadRequest("File size succed max value 5MB.");
            }

            if (file.ContentType != AzureConstants.BlobContentType)
            {
                return BadRequest("Only pdf files are allowed.");
            }

            var document = await _documentService.Create(file);
            return CreatedAtAction(nameof(Create), new { id = document.Id }, document);
        }

        [HttpPatch]
        public async Task<ActionResult<List<DocumentDTO>>> Update([FromBody] List<DocumentPatchModel> documents)
        {
            try
            {
                var updatedDocuments = await _documentService.UpdateDocumentsOrder(documents);
                return updatedDocuments;
            }
            catch (DocumentNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }

        }

        [HttpDelete]
        [Route("{name}/{id}")]
        public async Task<ActionResult> Delete(string name, string id)
        {
            try
            {
                await _documentService.Delete(name, id);
                return NoContent();
            }
            catch (DocumentNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                return NotFound(ex.Message);
            }
        }
    }
}