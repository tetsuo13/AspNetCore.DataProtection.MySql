using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace AspNetCore.DataProtection.MySql.Tests;

public class MySqlDataProtectionBuilderExtensionsTests
{
    [Fact]
    public void PersistKeysToMySql_UsesMySqlXmlRepository()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDataProtection().PersistKeysToMySql();
        var serviceProvider = serviceCollection.BuildServiceProvider(validateScopes: true);

        var keyManagementOptions = serviceProvider.GetRequiredService<IOptions<KeyManagementOptions>>();

        Assert.NotNull(keyManagementOptions.Value.XmlRepository);
        Assert.IsType<MySqlXmlRepository>(keyManagementOptions.Value.XmlRepository);
    }

    [Fact]
    public void PersistKeysToMySql_ThrowsIf_BuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => MySqlDataProtectionBuilderExtensions.PersistKeysToMySql(null!));
    }

    [Fact]
    public void PersistKeysToMySqlWithOptions_ThrowsIf_BuilderIsNull()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            MySqlDataProtectionBuilderExtensions.PersistKeysToMySql(null!, config =>
            {
                config.TableName = nameof(PersistKeysToMySqlWithOptions_ThrowsIf_BuilderIsNull);
            });
        });
    }

    [Fact]
    public void PersistKeysToMySqlWithOptions_UsesCustomTableName()
    {
        var expectedTableName = nameof(PersistKeysToMySqlWithOptions_UsesCustomTableName);
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDataProtection().PersistKeysToMySql(options =>
        {
            options.TableName = expectedTableName;
        });
        var serviceProvider = serviceCollection.BuildServiceProvider(validateScopes: true);

        var keyManagementOptions = serviceProvider.GetRequiredService<IOptions<KeyManagementOptions>>();
        var xmlRepository = keyManagementOptions.Value.XmlRepository as MySqlXmlRepository;

        Assert.NotNull(xmlRepository);
        Assert.Equal(expectedTableName, xmlRepository.TableName);
    }
}
