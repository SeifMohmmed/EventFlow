using System.Reflection;

namespace EventFlow.Modules.Events.Application;

/// <summary>
/// Provides access to the Events Application assembly.
/// Used for assembly scanning and dependency registration.
/// </summary>
public static class AssemblyReference
{
    /// <summary>
    /// Gets the Events Application assembly.
    /// </summary>
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
