using DocumentManagement.Common.Models;
using System;

namespace DocumentManagement.Common
{
    public static class ModelsMappingExtensions
    {
        public static DocumentDTO ToDTO(this DocumentEntity document)
        {
            return new DocumentDTO()
            {
                Id = Guid.Parse(document.RowKey),
                Name = document.PartitionKey,
                Location = document.Location,
                FileSize = document.FileSize
            };
        }
    }
}
