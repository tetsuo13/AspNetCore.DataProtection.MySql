using AspNetCore.DataProtection.MySql.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace AspNetCore.DataProtection.MySql;

public class MySqlXmlRepositoryTests
{
    [Fact]
    public void CreateRepository_ThrowsIf_ServiceProviderIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new MySqlXmlRepository(null!, NullLoggerFactory.Instance, new MySqlDataProtectionOptions());
        });
    }

    [Fact]
    public void CreateRepository_ThrowsIf_LoggerFactoryIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            new MySqlXmlRepository(serviceProvider, null!, new MySqlDataProtectionOptions());
        });
    }

    [Fact]
    public void CreateRepository_ThrowsIf_OptionsIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            new MySqlXmlRepository(serviceProvider, NullLoggerFactory.Instance, null!);
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

        Assert.Throws<ArgumentNullException>("configuration", () =>
        {
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            new MySqlXmlRepository(serviceProvider, NullLoggerFactory.Instance, configuration);
        });
    }
}
