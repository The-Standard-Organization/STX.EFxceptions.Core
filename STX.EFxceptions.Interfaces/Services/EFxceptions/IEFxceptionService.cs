// ----------------------------------------------------------------------------------
// Copyright(c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;

namespace STX.EFxceptions.Interfaces.Services.EFxceptions
{
    public interface IEFxceptionService<TException> where TException : Exception
    {
        void ThrowMeaningfulException(TException exception);
    }
}
