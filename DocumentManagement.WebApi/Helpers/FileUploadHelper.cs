using DocumentManagement.Common;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Helpers
{
    public class FileUploadHelper : IFileUploadHelper
    {
        private readonly AzureConfiguration _configuration;
        private AzureUtils _azureUtils;

        public FileUploadHelper(AzureConfiguration configuration)
        {
            _configuration = configuration;
            _azureUtils = new AzureUtils(_configuration.StorageConnectionString);
        }

        public async Task Upload(Stream file, string fileName)
        {
            var container = await _azureUtils.CreateIfNotExistsBlobContainer();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.UploadFromStream(file);
        }
    }
}
