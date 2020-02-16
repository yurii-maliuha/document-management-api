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
        Task<DocumentDTO> Create(IFormFile file);
    }
}
