using System.Reflection;

namespace EventFlow.Modules.Events.Presentation;

/// <summary>
/// Provides access to the Events Presentation assembly.
/// Used for assembly scanning and endpoint registration.
/// </summary>
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
