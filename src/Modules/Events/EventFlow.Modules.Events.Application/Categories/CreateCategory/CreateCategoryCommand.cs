using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.CreateCategory;

public sealed record CreateCategoryCommand(
    string Name) : ICommand<Guid>;
