// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;

namespace STX.EFxceptions.Abstractions.Models.Exceptions
{
    public class ForeignKeyConstraintConflictException : DbUpdateException
    {
        public ForeignKeyConstraintConflictException(string message) : base(message) { }

        public ForeignKeyConstraintConflictException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
