// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

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
    }
}
