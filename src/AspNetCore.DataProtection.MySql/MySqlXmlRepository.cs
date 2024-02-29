using AspNetCore.DataProtection.MySql.Models;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using System.Xml.Linq;

namespace AspNetCore.DataProtection.MySql;

/// <summary>
/// An <see cref="IXmlRepository"/> backed by MySQL.
/// </summary>
public class MySqlXmlRepository : IXmlRepository
{
    internal readonly string TableName;

    private string GetAllElementsSql => $"SELECT FriendlyName, Xml FROM {TableName}";
    private string StoreElementSql => $"INSERT INTO {TableName} (FriendlyName, Xml) VALUES (@FriendlyName, @Xml)";

    private readonly IServiceProvider _services;
    private readonly ILogger<MySqlXmlRepository> _logger;

    /// <summary>
    /// Initializes a new instance of the MySqlXmlRepository class.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="configuration"></param>
    /// <exception cref="ArgumentNullException">Thrown if any of the parameters are null.</exception>
    public MySqlXmlRepository(IServiceProvider services, ILoggerFactory loggerFactory,
        MySqlDataProtectionOptions configuration)
    {
        if (loggerFactory is null)
        {
            throw new ArgumentNullException(nameof(loggerFactory));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }
        else if (string.IsNullOrWhiteSpace(configuration.TableName))
        {
            throw new ArgumentNullException(nameof(configuration),
                $"Invalid {nameof(configuration.TableName)} option");
        }

        _services = services ?? throw new ArgumentNullException(nameof(services));
        _logger = loggerFactory.CreateLogger<MySqlXmlRepository>();
        TableName = configuration.TableName;
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<XElement> GetAllElements() =>
        GetAllElementsInternal().ToList().AsReadOnly();

    private IEnumerable<XElement> GetAllElementsInternal()
    {
        using var scope = _services.CreateScope();
        using var connection = scope.ServiceProvider.GetRequiredService<MySqlConnection>();
        connection.Open();

        using var command = new MySqlCommand(GetAllElementsSql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var xml = reader.GetString("Xml");

            _logger.ReadingXmlFromKey(reader.GetString("FriendlyName"));

            if (!string.IsNullOrEmpty(xml))
            {
                yield return XElement.Parse(xml);
            }
        }
    }

    /// <inheritdoc/>
    public void StoreElement(XElement element, string friendlyName)
    {
        using var scope = _services.CreateScope();
        using var connection = scope.ServiceProvider.GetRequiredService<MySqlConnection>();
        connection.Open();

        using var command = new MySqlCommand(StoreElementSql, connection);
        command.Parameters.AddWithValue("FriendlyName", friendlyName);
        command.Parameters.AddWithValue("Xml", element?.ToString(SaveOptions.DisableFormatting));

        _logger.LogSavingKey(friendlyName);

        command.ExecuteNonQuery();
    }
}
