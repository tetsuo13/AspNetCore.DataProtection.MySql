using AspNetCore.DataProtection.MySql.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AspNetCore.DataProtection.MySql.Tests;

public class MySqlXmlRepositoryTests
{
    [Fact]
    public void CreateRepository_ThrowsIf_ServiceProviderIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new MySqlXmlRepository(null!, NullLoggerFactory.Instance, new MySqlDataProtectionOptions());
        });
    }

    [Fact]
    public void CreateRepository_ThrowsIf_LoggerFactoryIsNull()
    {
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new MySqlXmlRepository(serviceProvider, null!, new MySqlDataProtectionOptions());
        });
    }

    [Fact]
    public void CreateRepository_ThrowsIf_OptionsIsNull()
    {
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>(() =>
        {
            var _ = new MySqlXmlRepository(serviceProvider, NullLoggerFactory.Instance, null!);
        });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void CreateRepository_ThrowsIf_EmptyTableNameInOptions(string? tableName)
    {
        var configuration = new MySqlDataProtectionOptions
        {
            TableName = tableName!
        };
        var serviceCollection = new ServiceCollection();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        Assert.Throws<ArgumentNullException>("configuration", () =>
        {
            var _ = new MySqlXmlRepository(serviceProvider, NullLoggerFactory.Instance, configuration);
        });
    }
}
