using System.Reflection;

namespace EventFlow.Modules.Ticketing.Application;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
