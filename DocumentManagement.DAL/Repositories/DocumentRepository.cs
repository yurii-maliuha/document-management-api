using DocumentManagement.DAL.Azure;
using DocumentManagement.DAL.Entities;
using DocumentManagement.Models.Exceptions;
using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.DAL.Repositories
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

        public async Task<List<DocumentEntity>> UpdateAsync(List<DocumentEntity> documents)
        {
            var updatedDocuments = new List<DocumentEntity>();
            foreach (var document in documents)
            {
                updatedDocuments.Add(await CreateOrUpdateAsync(document));
            }

            return updatedDocuments;
        }

        public async Task<bool> ExistsAsync(string name, string id)
        {
            var documentToDelete = await GetAsync(name, id);
            return documentToDelete != null;
        }

        public async Task DeleteAsync(string name, string id)
        {
            var documentToDelete = await GetAsync(name, id);
            var deleteOperation = TableOperation.Delete(documentToDelete);
            await _azureUtils.CloudTable.ExecuteAsync(deleteOperation);
        }

        private async Task<DocumentEntity> CreateOrUpdateAsync(DocumentEntity document)
        {
            var insertOrMergeOperation = TableOperation.InsertOrMerge(document);
            var result = await _azureUtils.CloudTable.ExecuteAsync(insertOrMergeOperation);
            var insertedDocument = result.Result as DocumentEntity;

            return insertedDocument;
        }
    }
}
