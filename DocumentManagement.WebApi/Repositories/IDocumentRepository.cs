using DocumentManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<DocumentEntity> GetAll();

        Task<DocumentEntity> CreateAsync(DocumentEntity document);

        Task DeleteAsync(string name, string id);

        Task<List<DocumentEntity>> UpdateDocuments(List<DocumentEntity> documents);
    }
}
