using DocumentManagement.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Repositories
{
    public interface IDocumentRepository
    {
        Task<DocumentEntity> AddOrUpdateAsync(DocumentEntity document);
    }
}
