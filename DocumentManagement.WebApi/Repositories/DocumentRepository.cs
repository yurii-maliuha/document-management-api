using DocumentManagement.Common;
using DocumentManagement.Common.Models;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private AzureUtils _azureUtils;

        public DocumentRepository(AzureUtils azureUtils)
        {
            _azureUtils = azureUtils;
        }

        public IEnumerable<DocumentEntity> GetAll()
        {
            var documents = _azureUtils.CloudTable
                .CreateQuery<DocumentEntity>()
                .AsEnumerable()
                .OrderBy(doc => doc.Order);

            return documents;
        }

        public async Task<DocumentEntity> GetAsync(string name, string id)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<DocumentEntity>(name, id);
            TableResult result = await _azureUtils.CloudTable.ExecuteAsync(retrieveOperation);
            DocumentEntity document = result.Result as DocumentEntity;

            return document;
        }

        public async Task<DocumentEntity> CreateAsync(DocumentEntity document)
        {
            var lastDocument = GetAll().LastOrDefault();
            document.Order = lastDocument != null ? lastDocument.Order + 1 : 0;
            var createdDocumet = await CreateOrUpdateAsync(document);

            return createdDocumet;
        }

        public async Task<List<DocumentEntity>> UpdateDocuments(List<DocumentPatchModel> documents)
        {
            var documentsEntity = GetAll();
            foreach (var document in documentsEntity)
            {
                var updatedDocument = documents.FirstOrDefault(doc => doc.Id == document.RowKey);
                if (updatedDocument == null)
                {
                    throw new DocumentNotFoundException("Unknown document was found in updated collection");
                }

                document.Order = documents.IndexOf(updatedDocument);
                await CreateOrUpdateAsync(document);
            }

            return GetAll().ToList();
        }

        public async Task DeleteAsync(string name, string id)
        {
            var documentToDelete = await GetAsync(name, id);
            if(documentToDelete == null)
            {
                throw new DocumentNotFoundException("Can not delete non existed file");
            }

            var deleteOperation = TableOperation.Delete(documentToDelete);
            await _azureUtils.CloudTable.ExecuteAsync(deleteOperation);
        }

        private async Task<DocumentEntity> CreateOrUpdateAsync(DocumentEntity document)
        {
            if (document == null)
            {
                throw new DocumentNotFoundException("Document entity can not be null.");
            }

            var insertOrMergeOperation = TableOperation.InsertOrMerge(document);
            var result = await _azureUtils.CloudTable.ExecuteAsync(insertOrMergeOperation);
            var insertedDocument = result.Result as DocumentEntity;

            return insertedDocument;
        }
    }
}
