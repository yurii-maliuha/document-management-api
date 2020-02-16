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
        public ActionResult<IEnumerable<DocumentDTO>> GetAll()
        {
            return Ok(_documentService.GetAll());
        }

        [HttpPost]
        public async Task<ActionResult<DocumentDTO>> Create(IFormFile file)
        {
            if (file.Length / 1000000 >= 5)
            {
                return BadRequest("File size succed max value 5MB.");
            }

            if (file.ContentType != "application/pdf")
            {
                return BadRequest("Only pdf files are allowed.");
            }

            var document = await _documentService.Create(file);
            return CreatedAtAction(nameof(Create), new { id = document.Id }, document);
        }

        [HttpPatch]
        public async Task<ActionResult<List<DocumentDTO>>> Update([FromBody] List<DocumentPatchModel> documents)
        {
            var updatedDocuments = await _documentService.UpdateDocuments(documents);
            return updatedDocuments;
        }

        [HttpDelete]
        [Route("{name}/{id}")]
        public async Task<ActionResult> Delete(string name, string id)
        {
            await _documentService.Delete(name, id);
            return NoContent();
        }
    }
}