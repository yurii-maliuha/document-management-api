using DocumentManagement.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DocumentManagement.DAL.Repositories
{
    public interface IDocumentRepository
    {
        IEnumerable<DocumentEntity> GetAll();

        Task<DocumentEntity> CreateAsync(DocumentEntity document);

        Task DeleteAsync(string name, string id);

        Task<DocumentEntity> UpdateAsync(DocumentEntity document);
    }
}
