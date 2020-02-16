using DocumentManagement.Common.Models;
using DocumentManagement.WebApi.Helpers;
using DocumentManagement.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IFileUploadHelper _fileUploadHelper;
        private readonly IDocumentRepository _documentRepository;

        public DocumentService(IFileUploadHelper fileUploadHelper,
            IDocumentRepository documentRepository)
        {
            _fileUploadHelper = fileUploadHelper;
            _documentRepository = documentRepository;
        }

        public async Task<DocumentDTO> Create(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                await _fileUploadHelper.Upload(memoryStream, file.FileName);
            }

            var documentEntity = new DocumentEntity(Guid.NewGuid(), file.FileName)
            {
                Name = file.FileName,
                FileSize = file.Length,
                Location = ""
            };

            var createdDocument = await _documentRepository.AddOrUpdateAsync(documentEntity);

            return new DocumentDTO()
            {
                Id = Guid.Parse(createdDocument.RowKey),
                Name = createdDocument.Name,
                Location = createdDocument.Location,
                FileSize = createdDocument.FileSize
            };
        }

        

    }
}
