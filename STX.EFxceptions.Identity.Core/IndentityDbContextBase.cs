// ----------------------------------------------------------------------------------
// Copyright (c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STX.EFxceptions.Abstractions.Brokers.DbErrorBroker;
using STX.EFxceptions.Abstractions.Services.EFxceptions;

namespace STX.EFxceptions.Identity.Core
{
    public abstract class IdentityDbContextBase<TUser> : IdentityDbContext<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
        protected IdentityDbContextBase() : base()
        { }

        public IdentityDbContextBase(DbContextOptions options) : base(options)
        { }
    }

    public abstract class IdentityDbContextBase<TUser, TRole, TKey, TException, TErrorCode>
        : IdentityDbContextBase<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>,
            IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>, TException, TErrorCode>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TException : Exception
    {
        protected IdentityDbContextBase() : base()
        { }

        public IdentityDbContextBase(DbContextOptions options) : base(options)
        { }
    }

    public abstract class IdentityDbContextBase<
        TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TException, TErrorCode>
        : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserRole : IdentityUserRole<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TRoleClaim : IdentityRoleClaim<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TException : Exception
    {
        private IEFxceptionService eFxceptionService;
        private IDbErrorBroker<TException, TErrorCode> errorBroker;

        protected IdentityDbContextBase() =>
            InitializeInternalServices();

        public IdentityDbContextBase(DbContextOptions options) : base(options) =>
            InitializeInternalServices();

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.eFxceptionService.ThrowMeaningfulException(
                    dbUpdateException);
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
            catch (DbUpdateException dbUpdateException)
            {
                this.eFxceptionService.ThrowMeaningfulException(dbUpdateException);
                throw;
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.eFxceptionService.ThrowMeaningfulException(dbUpdateException);
                throw;
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            try
            {
                return base.SaveChanges(acceptAllChangesOnSuccess);
            }
            catch (DbUpdateException dbUpdateException)
            {
                this.eFxceptionService.ThrowMeaningfulException(dbUpdateException);
                throw;
            }
        }

        protected abstract IDbErrorBroker<TException, TErrorCode> CreateErrorBroker();

        protected abstract IEFxceptionService CreateEFxceptionService(
            IDbErrorBroker<TException, TErrorCode> errorBroker);

        private void InitializeInternalServices()
        {
            errorBroker = CreateErrorBroker();
            eFxceptionService = CreateEFxceptionService(errorBroker);
        }
    }
}
