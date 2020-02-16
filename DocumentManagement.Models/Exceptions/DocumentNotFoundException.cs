using System;

namespace DocumentManagement.Models.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string message)
            : base(message)
        {

        }
    }
}
