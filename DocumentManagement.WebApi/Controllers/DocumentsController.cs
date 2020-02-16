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
    /// <summary>
    /// Documents Controller
    /// </summary>
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

        /// <summary>
        /// Return all documents
        /// </summary>
        /// <remarks>This endpoint returns the list of documents.</remarks>
        /// <returns>The list of documents.</returns>
        [HttpGet]
        public ActionResult<IEnumerable<DocumentDTO>> GetAll()
        {
            var documents = _documentService.GetAll();
            return Ok(documents);
        }

        /// <summary>
        /// Create document
        /// </summary>
        /// <remarks>This endpoint create uploaded document passed as parameter.</remarks>
        /// <param name="file">Uploaded file</param>
        /// <returns>Created document.</returns>
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
            return document;
        }

        /// <summary>
        /// Update documemts
        /// </summary>
        /// <remarks>This endpoint update documents value, currently only Order changing is allowed.</remarks>
        /// <param name="documents">Document patch models.</param>
        /// <returns>Updated list of documents.</returns>
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

        /// <summary>
        /// Delete documemt
        /// </summary>
        /// <remarks>This endpoint delete selected document.</remarks>
        /// <param name="name">Document name.</param>
        /// <param name="id">Document id.</param>
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