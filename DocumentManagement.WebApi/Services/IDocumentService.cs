using DocumentManagement.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Services
{
    public interface IDocumentService
    {
        IEnumerable<DocumentDTO> GetAll();

        Task<DocumentDTO> Create(IFormFile file);

        Task Delete(string name, string id);

        Task<List<DocumentDTO>> UpdateDocuments(List<DocumentDTO> documents);
    }
}
