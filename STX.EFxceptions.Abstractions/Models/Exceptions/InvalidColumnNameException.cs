﻿// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using Microsoft.EntityFrameworkCore;

namespace STX.EFxceptions.Abstractions.Models.Exceptions
{
    public class InvalidColumnNameException : DbUpdateException
    {
        public InvalidColumnNameException(string message) : base(message) { }
        public InvalidColumnNameException(string message, Exception innerException) : base(message, innerException) { }
    }
}
