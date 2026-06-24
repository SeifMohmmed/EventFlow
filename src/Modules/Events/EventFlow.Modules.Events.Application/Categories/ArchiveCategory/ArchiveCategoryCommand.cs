using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.ArchiveCategory;

public sealed record ArchiveCategoryCommand(
    Guid CategoryId) : ICommand;
