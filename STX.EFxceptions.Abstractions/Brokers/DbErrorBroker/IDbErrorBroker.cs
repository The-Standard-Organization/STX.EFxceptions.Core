// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.EFxceptions.Abstractions.Brokers.DbErrorBroker
{
    public interface IDbErrorBroker<TException, TErrorCode> where TException : Exception
    {
        TErrorCode GetErrorCode(TException exception);
    }
}
