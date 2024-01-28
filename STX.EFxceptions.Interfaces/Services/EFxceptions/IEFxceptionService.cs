﻿// ----------------------------------------------------------------------------------
// Copyright(c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using Microsoft.EntityFrameworkCore;

namespace STX.EFxceptions.Interfaces.Services.EFxceptions
{
    public interface IEFxceptionService
    {
        void ThrowMeaningfulException(DbUpdateException dbUpdateException);
    }
}
