using EventFlow.Modules.Events.Application.Abstractions.Messaging;
using EventFlow.Modules.Events.Application.Categories.GetCategory;

namespace EventFlow.Modules.Events.Application.Categories.GetCategories;

public sealed record GetCategoriesQuery
    : IQuery<IReadOnlyCollection<CategoryResponse>>;
