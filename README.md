<p align="center">
  <img width="25%" height="25%" src="https://github.com/The-Standard-Organization/STX.EFxceptions.Core/blob/main/Resources/EFxceptions.png?raw=true">
</p>

[![.Net](https://github.com/The-Standard-Organization/STX.EFxceptions.Core/actions/workflows/dotnet.yml/badge.svg)](https://github.com/The-Standard-Organization/STX.EFxceptions.Core/actions/workflows/dotnet.yml)
[![The Standard - COMPLIANT](https://img.shields.io/badge/The_Standard-COMPLIANT-2ea44f)](https://github.com/hassanhabib/The-Standard)


# STX.EFxceptions

We have designed and developed this library as a abstract base wrapper around the existing EntityFramework DbContext implementation to provide the following values:

<ol>
	<li>Meaningful exceptions for error codes.</li>
	<li>Simplified integrations</li>
	<li>Test-friendly implementation.</li>
</ol>

<br>

This solution consists of the following core projects:

<ul>
	<li><strong>STX.EFxceptions.Interfaces</strong>: A Standardized .NET library that provides interfaces for the core components to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.</li>
	<li><strong>STX.EFxceptions.Core</strong>: A Standardized .NET library that provides an abstract DBContext to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.</li>
	<li><strong>STX.EFxceptions.Identity.Core</strong>: A Standardized .NET library that provides an abstract DBContext that implements AspNetCore.Identity to captures exceptions thrown by EntityFramework and converts them into meaningful exceptions.</li>
</ul>


## STX.EFxceptions.Interfaces

[![preview version](https://img.shields.io/nuget/vpre/STX.EFxceptions.Interfaces)](https://www.nuget.org/packages/STX.EFxceptions.Interfaces/absoluteLatest)

This library provides the interfaces which are used by the core components to get error codes and provide meaningful exceptions.

## STX.EFxceptions.Core

[![preview version](https://img.shields.io/nuget/vpre/STX.EFxceptions.Core)](https://www.nuget.org/packages/STX.EFxceptions.Core/absoluteLatest)

This library provides an abstract DBContext which is used to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.
Custom implementations for the various Entity Framework database providers should inherit from this abstract class.

## STX.EFxceptions.Identity.Core

[![preview version](https://img.shields.io/nuget/vpre/STX.EFxceptions.Identity.Core)](https://www.nuget.org/packages/STX.EFxceptions.Identity.Core/absoluteLatest)

This library provides an abstract DBContext that inherits from `Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext` which is used to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.
Custom implementations for the various Entity Framework database providers that make use of Microsoft.AspNetCore.Identity should inherit from this abstract class.


# How to use

These core libraries are designed to be used as a base class for your DbContext implementation.  If you would like to create your own custom DbContext implementation, you can inherit from the abstract base class provided by this library.

## Example

Lets use SQL Server as an example.  If you would like to create a custom DbContext implementation that uses SQL Server, you can inherit from the abstract base class provided by this library.

```cs
    public class EFxceptionsContext : DbContextBase<SqlException>
    {
        protected override IDbErrorBroker<SqlException> CreateErrorBroker()
        {
            return new SqlErrorBroker();
        }

        protected override IDbErrorBroker<SqlException> CreateErrorBroker() =>
            new SqlServerErrorBroker();

        protected override IEFxceptionService CreateEFxceptionService(IDbErrorBroker<SqlException> errorBroker)
        {
            return new SqlServerEFxceptionService<SqlException>(errorBroker);
        }
    }
```

Next we will have to do an implementation of the `IDbErrorBroker` interface.  This broker is used to get the error code for the exceptions that can be thrown by EntityFramework.

```cs
    public interface ISqlServerErrorBroker : IDbErrorBroker<SqlException>
    { }

    public class SqlServerErrorBroker : ISqlServerErrorBroker
    {
        public int GetErrorCode(SqlException exception) => exception.Number;
    }
```

Finally, we will have to do an implementation of the `IEFxceptionService` interface and service.  This interface and service is used to provide the meaningful exceptions for the various error codes that can be thrown by EntityFramework.

```cs
	public interface ISqlServerEFxceptionService : IEFxceptionService<SqlException>
	{ }

	public class SqlServerEFxceptionService<TException> : ISqlServerEFxceptionService
	{
		public SqlServerEFxceptionService(IDbErrorBroker<TException> errorBroker) : base(errorBroker)
		{ }

        public void ThrowMeaningfulException(SqlException sqlException)
        {
            ValidateInnerException(sqlException);
            SqlException innerException = GetException(sqlException.InnerException);
            int errorCode = this.errorBroker.GetErrorCode(innerException);
            ConvertAndThrowMeaningfulException(sqlErrorCode, dbException.Message);

            throw dbUpdateException;
        }

        private void ValidateInnerException(SqlException sqlException)
        {
            if (SqlException.InnerException == null)
            {
                throw sqlException;
            }
        }

        private void ConvertAndThrowMeaningfulException(int code, string message)
        {
            switch (code)
            {
                case 207:
                    throw new InvalidColumnNameException(message);
                case 208:
                    throw new InvalidObjectNameException(message);
                case 547:
                    throw new ForeignKeyConstraintConflictException(message);
                case 2601:
                    throw new DuplicateKeyWithUniqueIndexException(message);
                case 2627:
                    throw new DuplicateKeyException(message);
            }
        }

        private SqlException GetSqlException(Exception exception) => (SqlException)exception;
```

# Current Implementations

[STX.EFxceptions.SqlServer](https://github.com/The-Standard-Organization/STX.EFxceptions.SqlServer)

[STX.EFxceptions.SQLite](https://github.com/The-Standard-Organization/STX.EFxceptions.SQLite)

[STX.EFxceptions.MySql](https://github.com/The-Standard-Organization/STX.EFxceptions.MySql)

[STX.EFxceptions.Cosmos](https://github.com/The-Standard-Organization/STX.EFxceptions.Cosmos)
