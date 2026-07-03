using Newtonsoft.Json;

namespace EventFlow.Common.Infrastructure.Serialization;

public static class SerializerSettings
{
    // Shared JSON serialization settings used throughout the application.
    public static readonly JsonSerializerSettings Instance = new()
    {
        // Include CLR type information so polymorphic objects
        // (such as domain events) can be deserialized correctly.
        TypeNameHandling = TypeNameHandling.All,

        // Read metadata properties (such as $type) before other JSON properties
        // to ensure type information is available during deserialization.
        MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead
    };
}
