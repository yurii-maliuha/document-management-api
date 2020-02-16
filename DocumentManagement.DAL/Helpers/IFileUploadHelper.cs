using System.IO;
using System.Threading.Tasks;

namespace DocumentManagement.DAL.Helpers
{
    public interface IFileUploadHelper
    {
        Task<string> Upload(Stream file, string fileName);

        Task<bool> ExistsAsync(string blobName);

        Task DeleteAsync(string blobName);
    }
}
