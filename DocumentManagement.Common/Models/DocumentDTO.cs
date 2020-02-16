using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.Common.Models
{
    public class DocumentDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public long FileSize { get; set; }
    }
}
