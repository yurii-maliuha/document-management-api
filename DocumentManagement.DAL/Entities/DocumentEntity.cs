using Microsoft.Azure.Cosmos.Table;
using System;

namespace DocumentManagement.DAL.Entities
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

        public string Location { get; set; }

        public long FileSize { get; set; }

        public int Order { get; set; }
    }
}
