// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace STX.EFxceptions.Abstractions.Models.Exceptions
{
    public class InvalidObjectNameException : DbUpdateException
    {
        public InvalidObjectNameException(string message) : base(message) { }
    }
}
