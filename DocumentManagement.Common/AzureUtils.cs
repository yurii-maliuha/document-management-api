using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;

namespace DocumentManagement.Common
{
    public class AzureUtils
    {
        private readonly string _connectionString;
        public AzureUtils(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<CloudBlobContainer> CreateIfNotExistsBlobContainer()
        {
            var storageAccount = Microsoft.Azure.Storage.CloudStorageAccount.Parse(_connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(AzureConstants.BlobContainerName);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        public async Task<CloudTable> CreateIfNotExistsTable()
        {
            CloudTable table = null;
            try
            {
                var storageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(_connectionString);
                var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                table = tableClient.GetTableReference(AzureConstants.CosmoTableName);
                await table.CreateIfNotExistsAsync();

            }
            catch(Exception ex)
            {
                var exx = ex;
            }

            return table;
        }
    }
}
