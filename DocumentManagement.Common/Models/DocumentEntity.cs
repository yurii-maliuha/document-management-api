using Microsoft.Azure.Cosmos.Table;
using System;

namespace DocumentManagement.Common.Models
{
    public class DocumentEntity: TableEntity
    {
        public DocumentEntity()
        {
        }

        public DocumentEntity(Guid Id, string Name)
        {
            PartitionKey = Name;
            RowKey = Id.ToString();
        }

        public string Name { get; set; }

        public string Location { get; set; }

        public long FileSize { get; set; }
    }
}
