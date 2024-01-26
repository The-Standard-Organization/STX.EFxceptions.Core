// ----------------------------------------------------------------------------------
// Copyright(c) The Standard Organization: A coalition of the Good-Hearted Engineers
// ----------------------------------------------------------------------------------

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using STX.EFxceptions.Interfaces.Brokers.DbErrorBroker;
using STX.EFxceptions.Interfaces.Services.EFxceptions;

namespace STX.EFxceptions.Identity.Core.Brokers.IdentityDbContextBases
{
    public class IdentityDbContextBase<TUser> : IdentityDbContext<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
        protected IdentityDbContextBase()
        { }

        public IdentityDbContextBase(DbContextOptions options) : base(options)
        { }
    }

    public abstract class IdentityDbContextBase<TUser, TRole, TKey, TException>
        : IdentityDbContextBase<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>,
            IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>, TException>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
        where TException : Exception
    {
        protected IdentityDbContextBase()
        { }

        public IdentityDbContextBase(DbContextOptions options) : base(options)
        { }
    }

    public abstract class IdentityDbContextBase<
        TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TException>
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
        private IEFxceptionService<TException> eFxceptionService;
        private IDbErrorBroker<TException> errorBroker;

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

        protected abstract IDbErrorBroker<TException> CreateErrorBroker();

        protected abstract IEFxceptionService<TException> CreateEFxceptionService(
            IDbErrorBroker<TException> errorBroker);

        private void InitializeInternalServices()
        {
            this.errorBroker = CreateErrorBroker();
            this.eFxceptionService = CreateEFxceptionService(this.errorBroker);
        }
    }
}
