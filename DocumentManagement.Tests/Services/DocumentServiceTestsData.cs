using DocumentManagement.Common.Models;
using DocumentManagement.DAL.Entities;
using DocumentManagement.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentManagement.Tests.Services
{
    public static class DocumentServiceTestsData
    {
        public static List<DocumentEntity> UnsortedDocuments
        {
            get
            {
                return new List<DocumentEntity>()
                {
                    new DocumentEntity(Guid.Parse("9f44e14c-1c3a-4795-9827-83cb9fefc5c4"), "TestDocument2")
                    {
                        FileSize = 202020,
                        Location = "",
                        Order = 0
                    },
                    new DocumentEntity(Guid.Parse("818861d9-49cb-40c1-851f-ccc2a46e1ce2"), "TestDocument1")
                    {
                        FileSize = 202020,
                        Location = "",
                        Order = 1
                    }
                };
            }
        }

        public static List<DocumentEntity> SortedDocuments
        {
            get
            {
                return new List<DocumentEntity>()
                {
                    new DocumentEntity(Guid.Parse("818861d9-49cb-40c1-851f-ccc2a46e1ce2"), "TestDocument1")
                    {
                        FileSize = 202020,
                        Location = "",
                         Order = 0
                    },
                    new DocumentEntity(Guid.Parse("9f44e14c-1c3a-4795-9827-83cb9fefc5c4"), "TestDocument2")
                    {
                        FileSize = 202020,
                        Location = "",
                         Order = 1
                    }
                };
            }
        }

        public static List<DocumentPatchModel> DocumentPatchModels
        {
            get
            {
                return new List<DocumentPatchModel>()
                {
                    new DocumentPatchModel { Id = "818861d9-49cb-40c1-851f-ccc2a46e1ce2"},
                    new DocumentPatchModel { Id = "9f44e14c-1c3a-4795-9827-83cb9fefc5c4"}
                };
            }
        }
    }
}
