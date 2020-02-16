using DocumentManagement.DAL.Azure;
using DocumentManagement.Models.Exceptions;
using Microsoft.Azure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.DAL.Helpers
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
            blockBlob.Properties.ContentType = AzureConstants.BlobContentType;
            await blockBlob.UploadFromStreamAsync(file);
            return blockBlob.Uri.AbsoluteUri;
        }

        public Task<bool> ExistsAsync(string blobName)
        {
            CloudBlockBlob blockBlob = _azureUtils.BlobContainer.GetBlockBlobReference(blobName);
            return blockBlob.ExistsAsync();
        }

        public async Task DeleteAsync(string blobName)
        {
            CloudBlockBlob blockBlob = _azureUtils.BlobContainer.GetBlockBlobReference(blobName);
            await blockBlob.DeleteAsync();
        }
    }
}
