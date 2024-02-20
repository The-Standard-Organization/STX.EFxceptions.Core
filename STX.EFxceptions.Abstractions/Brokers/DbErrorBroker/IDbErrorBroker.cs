// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.EFxceptions.Abstractions.Brokers.DbErrorBroker
{
    public interface IDbErrorBroker<TException> where TException : Exception
    {
        int GetErrorCode(TException exception);
    }
}
