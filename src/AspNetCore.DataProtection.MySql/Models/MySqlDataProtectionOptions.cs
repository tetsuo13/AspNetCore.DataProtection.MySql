namespace AspNetCore.DataProtection.MySql.Models;

/// <summary>
/// Options class that provides information needed for MySQL data protection.
/// </summary>
public class MySqlDataProtectionOptions
{
    /// <summary>
    /// The name of the table to use.
    /// </summary>
    public string TableName { get; set; } = "DataProtectionKeys";
}
