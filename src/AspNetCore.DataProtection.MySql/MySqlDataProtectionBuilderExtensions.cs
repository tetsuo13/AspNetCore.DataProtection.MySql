using AspNetCore.DataProtection.MySql.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace AspNetCore.DataProtection.MySql;

/// <summary>
/// Extension methods for configuring instances of <see cref="MySqlXmlRepository"/>.
/// </summary>
public static class MySqlDataProtectionBuilderExtensions
{
    /// <summary>
    /// Configures the data protection system to persist keys to MySQL.
    /// </summary>
    /// <param name="builder">The <see cref="IDataProtectionBuilder"/> instance to modify.</param>
    /// <returns>The value <paramref name="builder"/>.</returns>
    public static IDataProtectionBuilder PersistKeysToMySql(this IDataProtectionBuilder builder) =>
        PersistKeysToMySql(builder, _ => { });

    /// <summary>
    /// Configures the data protection system to persist keys to MySQL.
    /// </summary>
    /// <param name="builder">The <see cref="IDataProtectionBuilder"/> instance to modify.</param>
    /// <param name="configureOptions">A delegate that allows configuring <see cref="MySqlDataProtectionOptions"/>.</param>
    /// <returns>The value <paramref name="builder"/>.</returns>
    public static IDataProtectionBuilder PersistKeysToMySql(this IDataProtectionBuilder builder,
        Action<MySqlDataProtectionOptions> configureOptions)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
        {
            var defaultOptions = new MySqlDataProtectionOptions();
            configureOptions.Invoke(defaultOptions);

            var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

            return new ConfigureOptions<KeyManagementOptions>(options =>
            {
                options.XmlRepository = new MySqlXmlRepository(services, loggerFactory, defaultOptions);
            });
        });

        return builder;
    }
}
