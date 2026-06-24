using EventFlow.Modules.Events.Application.Abstractions.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(
    Guid CategoryId) : IQuery<CategoryResponse>;
