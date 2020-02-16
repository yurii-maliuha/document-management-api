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
        public DocumentRepository(AzureConfiguration configuration)
        {
            _azureUtils = new AzureUtils(configuration.StorageConnectionString);
        }

        public async Task<DocumentEntity> AddOrUpdateAsync(DocumentEntity document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("Document entity can not be null.");
            }

            var documentTable = await _azureUtils.CreateIfNotExistsTable();
            var insertOrMergeOperation = TableOperation.InsertOrMerge(document);
            var result = await documentTable.ExecuteAsync(insertOrMergeOperation);
            var insertedDocument = result.Result as DocumentEntity;

            return insertedDocument;
        }
    }
}
