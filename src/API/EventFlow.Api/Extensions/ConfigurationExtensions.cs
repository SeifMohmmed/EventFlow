namespace EventFlow.Api.Extensions;

/// <summary>
/// Provides extension methods for loading
/// module-specific configuration files.
/// </summary>
internal static class ConfigurationExtensions
{
    /// <summary>
    /// Adds configuration files for each module.
    /// </summary>
    internal static void AddModuleConfiguration(
        this IConfigurationBuilder configurationBuilder,
        string[] modules)
    {
        foreach (string module in modules)
        {
            // Load the module's base configuration.
            configurationBuilder.AddJsonFile(
                $"modules.{module}.json",
                optional: false,
                reloadOnChange: true);

            // Load environment-specific overrides if available.
            configurationBuilder.AddJsonFile(
                $"modules.{module}.Development.json",
                optional: true,
                reloadOnChange: true);
        }
    }
}
