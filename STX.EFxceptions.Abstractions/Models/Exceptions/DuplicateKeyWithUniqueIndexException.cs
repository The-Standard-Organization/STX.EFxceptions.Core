// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;

namespace STX.EFxceptions.Abstractions.Models.Exceptions
{
    public class DuplicateKeyWithUniqueIndexException : DbUpdateException
    {
        public string DuplicateKeyValue { get; }

        public DuplicateKeyWithUniqueIndexException(string message)
            : base(message)
        {
            string[] subStrings = message.Split('(', ')');

            if (subStrings.Length == 3)
            {
                DuplicateKeyValue = subStrings[1];
            }
        }

        public DuplicateKeyWithUniqueIndexException(string message, Exception innerException)
            : base(message, innerException)
        {
            string[] subStrings = message.Split('(', ')');

            if (subStrings.Length == 3)
            {
                DuplicateKeyValue = subStrings[1];
            }
        }
    }
}
