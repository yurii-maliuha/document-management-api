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
        private AzureUtils _azureUtils;

        public FileUploadHelper(AzureUtils azureUtils)
        {
            _azureUtils = azureUtils;
        }

        public async Task<string> Upload(Stream file, string blobName)
        {
            var blockBlob = _azureUtils.BlobContainer.GetBlockBlobReference(blobName);
            blockBlob.Properties.ContentType = "application/pdf";
            await blockBlob.UploadFromStreamAsync(file);
            return blockBlob.Uri.AbsoluteUri;
        }

        public async Task Delete(string blobName)
        {
            CloudBlockBlob blockBlob = _azureUtils.BlobContainer.GetBlockBlobReference(blobName);
            await blockBlob.DeleteIfExistsAsync();
        }
    }
}
