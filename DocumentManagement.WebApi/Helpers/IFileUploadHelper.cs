using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentManagement.WebApi.Helpers
{
    public interface IFileUploadHelper
    {
        Task<string> Upload(Stream file, string fileName);

        Task Delete(string blobName);
    }
}
