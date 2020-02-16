﻿using System;

namespace DocumentManagement.Models
{
    public class DocumentDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Location { get; set; }

        public long FileSize { get; set; }
    }
}
