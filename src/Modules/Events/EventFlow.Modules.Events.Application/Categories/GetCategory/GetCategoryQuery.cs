using EventFlow.Common.Application.Messaging;

namespace EventFlow.Modules.Events.Application.Categories.GetCategory;

public sealed record GetCategoryQuery(
    Guid CategoryId) : IQuery<CategoryResponse>;
