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
        private CloudBlobContainer _blobContainer;
        private CloudTable _cloudTable;

        public CloudBlobContainer BlobContainer
        {
            get
            {
                if (_blobContainer == null)
                {
                    _blobContainer = CreateIfNotExistsBlobContainer().Result;
                }

                return _blobContainer;
            }
        }

        public CloudTable CloudTable
        {
            get
            {
                if (_cloudTable == null)
                {
                    _cloudTable = CreateIfNotExistsTable().Result;
                }

                return _cloudTable;
            }
        }

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
            var contPermission = new BlobContainerPermissions() { PublicAccess = BlobContainerPublicAccessType.Blob };
            container.SetPermissions(contPermission);
            return container;
        }

        public async Task<CloudTable> CreateIfNotExistsTable()
        {
            var storageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.Parse(_connectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var table = tableClient.GetTableReference(AzureConstants.CosmoTableName);
            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
