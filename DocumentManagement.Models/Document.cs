using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.Models
{
    public class Document
    {
        public string Name { get; set; }

        public string Location { get; set; }

        public int FileSize { get; set; }
    }
}
