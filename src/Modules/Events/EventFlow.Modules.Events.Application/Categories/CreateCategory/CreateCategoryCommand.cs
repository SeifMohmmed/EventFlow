using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name) : ICommand<Guid>;
