using DocumentManagement.Common;
using DocumentManagement.Common.Models;
using DocumentManagement.WebApi.Helpers;
using DocumentManagement.WebApi.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public IEnumerable<DocumentDTO> GetAll()
        {
            var documents =_documentRepository.GetAll()
                .Select(document => document.ToDTO());

            return documents;
        }

        public async Task<DocumentDTO> Create(IFormFile file)
        {
            var documentId = Guid.NewGuid();
            var documentLocation = "";
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                var blobName = $"{documentId}_{file.FileName}";
                documentLocation = await _fileUploadHelper.Upload(memoryStream, blobName);
            }

            var documentEntity = new DocumentEntity(documentId, file.FileName)
            {
                FileSize = file.Length,
                Location = documentLocation
            };

            var createdDocument = await _documentRepository.CreateAsync(documentEntity);

            return createdDocument.ToDTO();
        }

        public async Task<List<DocumentDTO>> UpdateDocuments(List<DocumentPatchModel> documents)
        {
            var updatedDocuments = await _documentRepository.UpdateDocuments(documents);
            return updatedDocuments.Select(document => document.ToDTO()).ToList();
        }

        public async Task Delete(string name, string id)
        {
            var blobName = $"{id}_{name}";
            await _fileUploadHelper.Delete(blobName);
            await _documentRepository.DeleteAsync(name, id);
        }
    }
}
