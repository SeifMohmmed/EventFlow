using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.UpdateCategory;

public sealed record UpdateCategoryCommand(
    Guid CategoryId, string Name) : ICommand;
