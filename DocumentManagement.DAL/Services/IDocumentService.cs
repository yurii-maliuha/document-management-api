using DocumentManagement.Common.Models;
using DocumentManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.DAL.Services
{
    public interface IDocumentService
    {
        IEnumerable<DocumentDTO> GetAll();

        Task<DocumentDTO> Create(IFormFile file);

        Task Delete(string name, string id);

        Task<List<DocumentDTO>> UpdateDocumentsOrder(List<DocumentPatchModel> documents);
    }
}
