// ----------------------------------------------------------------------------------
// Copyright(c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using STX.EFxceptions.Interfaces.Brokers.DbErrorBroker;
using STX.EFxceptions.Interfaces.Services.EFxceptions;

namespace STX.EFxceptions.Core.Brokers.DbContextBases
{
    public abstract class DbContextBase<TException> : DbContext
    where TException : Exception
    {
        private IEFxceptionService<TException> eFxceptionService;
        private IDbErrorBroker<TException> errorBroker;

        protected DbContextBase() =>
            InitializeInternalServices();

        public DbContextBase(DbContextOptions options)
            : base(options) => InitializeInternalServices();

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (TException tException)
            {
                this.eFxceptionService.ThrowMeaningfulException(
                    tException);
                throw;
            }
        }

        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            }
            catch (TException tException)
            {
                this.eFxceptionService.ThrowMeaningfulException(tException);
                throw;
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (TException tException)
            {
                this.eFxceptionService.ThrowMeaningfulException(tException);
                throw;
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (TException tException)
            {
                this.eFxceptionService.ThrowMeaningfulException(tException);
                throw;
            }
        }

        private void InitializeInternalServices()
        {
            this.errorBroker = CreateErrorBroker();
            this.eFxceptionService = CreateEFxceptionService(this.errorBroker);
        }

        protected abstract IDbErrorBroker<TException> CreateErrorBroker();
        protected abstract IEFxceptionService<TException> CreateEFxceptionService(
            IDbErrorBroker<TException> errorBroker);
    }
}
