using DocumentManagement.Common.Models;
using DocumentManagement.DAL.Entities;
using DocumentManagement.DAL.Extensions;
using DocumentManagement.DAL.Helpers;
using DocumentManagement.DAL.Repositories;
using DocumentManagement.Models;
using DocumentManagement.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.DAL.Services
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

        public async Task<List<DocumentDTO>> UpdateDocumentsOrder(List<DocumentPatchModel> documents)
        {
            var documentEntities = new List<DocumentEntity>();
            var documentsEntity = _documentRepository.GetAll();
            foreach (var document in documentsEntity)
            {
                var documentPatch = documents.FirstOrDefault(doc => doc.Id == document.RowKey);
                if (documentPatch == null)
                {
                    throw new DocumentNotFoundException($"Failed to updated document order with id {document.RowKey}.");
                }

                document.Order = documents.IndexOf(documentPatch);
                documentEntities.Add(await _documentRepository.UpdateAsync(document));
            }

            var updatedDocument = documentEntities.OrderBy(d => d.Order)
                .Select(d => d.ToDTO())
                .ToList();

            return updatedDocument;
        }

        public async Task Delete(string name, string id)
        {
            var blobName = $"{id}_{name}";
            if (!(await _fileUploadHelper.ExistsAsync(blobName)))
            {
                throw new DocumentNotFoundException($"Can not delete non existed file {blobName}");
            }

            await _fileUploadHelper.DeleteAsync(blobName);

            if (! (await _documentRepository.ExistsAsync(name, id)))
            {
                throw new DocumentNotFoundException($"Cannot delete the nonexistent file {name} : {id}");
            }

            await _documentRepository.DeleteAsync(name, id);
        }
    }
}
