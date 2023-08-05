# ASP.NET Core Data Protection for MySQL

[![Continuous integration](https://github.com/tetsuo13/AspNetCore.DataProtection.MySql/actions/workflows/ci.yml/badge.svg)](https://github.com/tetsuo13/AspNetCore.DataProtection.MySql/actions/workflows/ci.yml) [![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

An ASP.NET Core [Data Protection](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction) provider for MySQL using [MySqlConnector](https://mysqlconnector.net/).

## Getting Started

```csharp
using AspNetCore.DataProtection.MySql;
using Microsoft.Extensions.DependencyInjection;

public void ConfigureServices(IServiceCollection services)
{
    services.AddDataProtection()
        .PersistKeysToMySql();
}
```

The schema for the `DataProtectionKeys` table is below. This table has to be created ahead of time as this library won't automatically create it. The table name can be changed (and supplied to the overloaded `.PersistKeysToMySql()` method) however the column names are expected to match the default schema.

```sql
CREATE TABLE DataProtectionKeys (
    Id BIGINT NOT NULL AUTO_INCREMENT,
    FriendlyName TEXT NULL,
    Xml TEXT NULL,

    PRIMARY KEY CLUSTERED (Id ASC)
) DEFAULT CHARSET=utf8
COMMENT = 'Cryptographic keys for the data protection system';
```

Note: See [key storage providers in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/implementation/key-storage-providers) for other providers.

## Configuration

The overload to `.PersistKeysToMySql()` accepts a parameter to customize behavior:

```csharp
services.AddDataProtection()
    .PersistKeysToMySql(options =>
    {
        // Change the default table name to something else.
        options.TableName = "AlternateTableName";
    });
```

There are no options available to configure how it should connect to the database as it assumes that `MySqlConnection` has already been registered as a service dependency. As a result, it will request a `MySqlConnector` object from the service registry and run with it.

An example of how to register `MySqlConnector` as a service:

```csharp
var connectionString = configuration.GetValue<string>("ConnectionStrings:ConnectionString");

services.AddTransient(container =>
{
    return new MySqlConnection(connectionString);
});
```

## License

See [LICENSE](LICENSE) for more information.
