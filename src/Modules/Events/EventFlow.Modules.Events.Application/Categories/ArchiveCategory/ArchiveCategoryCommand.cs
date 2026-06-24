using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(
    Guid CategoryId) : ICommand;
