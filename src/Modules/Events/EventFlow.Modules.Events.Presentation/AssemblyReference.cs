using System.Reflection;

namespace EventFlow.Modules.Events.Presentation;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
