![STX.EFxceptions.Core](https://raw.githubusercontent.com/The-Standard-Organization/STX.EFxceptions.Core/main/Resources/Images/stx.efCore_git_logo.png)

[![Build](https://github.com/The-Standard-Organization/STX.EFxceptions.Core/actions/workflows/dotnet.yml/badge.svg)](https://github.com/The-Standard-Organization/STX.EFxceptions.Core/actions/workflows/dotnet.yml)[![The Standard](https://img.shields.io/github/v/release/hassanhabib/The-Standard?filter=v2.10.0&style=default&label=Standard%20Version&color=2ea44f)](https://github.com/hassanhabib/The-Standard)
[![The Standard - COMPLIANT](https://img.shields.io/badge/The_Standard-COMPLIANT-2ea44f)](https://github.com/hassanhabib/The-Standard)
[![The Standard Community](https://img.shields.io/discord/934130100008538142?color=%237289da&label=The%20Standard%20Community&logo=Discord)](https://discord.gg/vdPZ7hS52X)

# STX.EFxceptions.Core

## Introduction
We have designed and developed this library as a abstract base wrapper around the existing EntityFramework DbContext implementation to provide the following values:

1. Meaningful exceptions for error codes.
2. Simplified integrations
3. Test-friendly implementation.

This solution consists of the following core projects:

- A ***STX.EFxceptions.Interfaces*** : Standardized .NET library that provides interfaces for the core components to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.
- A ***STX.EFxceptions.Core*** : Standardized .NET library that provides an abstract DBContext to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.
- A ***STX.EFxceptions.Identity.Core*** : Standardized .NET library that provides an abstract DBContext that implements AspNetCore.Identity to captures exceptions thrown by EntityFramework and converts them into meaningful exceptions.

## Standard-Compliance
This library was built according to The Standard. The library follows engineering principles, patterns and tooling as recommended by The Standard.

This library is also a community effort which involved many nights of pair-programming, test-driven development and in-depth exploration research and design discussions.

## Standard-Promise
The most important fulfillment aspect in a Standard complaint system is aimed towards contributing to people, its evolution, and principles.
An organization that systematically honors an environment of learning, training, and sharing knowledge is an organization that learns from the past, makes calculated risks for the future, 
and brings everyone within it up to speed on the current state of things as honestly, rapidly, and efficiently as possible. 
 
We believe that everyone has the right to privacy, and will never do anything that could violate that right.
We are committed to writing ethical and responsible software, and will always strive to use our skills, coding, and systems for the good.
We believe that these beliefs will help to ensure that our software(s) are safe and secure and that it will never be used to harm or collect personal data for malicious purposes.
 
The Standard Community as a promise to you is in upholding these values.

## Get the Packages

### STX.EFxceptions.Interfaces

[![preview version](https://img.shields.io/nuget/vpre/STX.EFxceptions.Interfaces)](https://www.nuget.org/packages/STX.EFxceptions.Interfaces/absoluteLatest)

This library provides the interfaces which are used by the core components to get error codes and provide meaningful exceptions.

### STX.EFxceptions.Core

[![preview version](https://img.shields.io/nuget/vpre/STX.EFxceptions.Core)](https://www.nuget.org/packages/STX.EFxceptions.Core/absoluteLatest)

This library provides an abstract DBContext which is used to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.
Custom implementations for the various Entity Framework database providers should inherit from this abstract class.

### STX.EFxceptions.Identity.Core

[![preview version](https://img.shields.io/nuget/vpre/STX.EFxceptions.Identity.Core)](https://www.nuget.org/packages/STX.EFxceptions.Identity.Core/absoluteLatest)

This library provides an abstract DBContext that inherits from `Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext` which is used to capture exceptions thrown by EntityFramework and converts them into meaningful exceptions.
Custom implementations for the various Entity Framework database providers that make use of Microsoft.AspNetCore.Identity should inherit from this abstract class.


# How to use

These core libraries are designed to be used as a base class for your DbContext implementation.  If you would like to create your own custom DbContext implementation, you can inherit from the abstract base class provided by this library.

## Example

Lets use SQL Server as an example.  If you would like to create a custom implementation that uses SQL Server, you can inherit from the abstract base class provided by this library.

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
                    throw new InvalidColumnNameSqlServerException(message);
                case 208:
                    throw new InvalidObjectNameSqlServerException(message);
                case 547:
                    throw new ForeignKeyConstraintConflictSqlServerException(message);
                case 2601:
                    throw new DuplicateKeyWithUniqueIndexSqlServerException(message);
                case 2627:
                    throw new DuplicateKeySqlServerException(message);
            }
        }

        private SqlException GetSqlException(Exception exception) => (SqlException)exception;
```

# Current Implementations

[STX.EFxceptions.SqlServer](https://github.com/The-Standard-Organization/STX.EFxceptions.SqlServer)

[STX.EFxceptions.SQLite](https://github.com/The-Standard-Organization/STX.EFxceptions.SQLite)

[STX.EFxceptions.MySql](https://github.com/The-Standard-Organization/STX.EFxceptions.MySql)

[STX.EFxceptions.Cosmos](https://github.com/The-Standard-Organization/STX.EFxceptions.Cosmos)

## Contact

If you have any suggestions, comments or questions, please feel free to contact me on:

[Twitter](https://twitter.com/hassanrezkhabib)

[LinkedIn](https://www.linkedin.com/in/hassanrezkhabib/)

[E-Mail](mailto:hassanhabib@live.com)

### Important Notice
A special thanks to Mr. Hassan Habib and Mr. Christo du Toit for their continuing dedicated contributions.