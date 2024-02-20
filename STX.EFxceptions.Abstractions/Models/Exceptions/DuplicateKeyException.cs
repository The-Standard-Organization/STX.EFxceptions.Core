// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;

namespace STX.EFxceptions.Abstractions.Models.Exceptions
{
    public class DuplicateKeyException : DbUpdateException
    {
        public DuplicateKeyException(string message) : base(message) { }

        public DuplicateKeyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
