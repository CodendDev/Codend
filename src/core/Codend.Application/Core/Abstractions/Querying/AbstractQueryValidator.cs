using Codend.Application.Core.Abstractions.Messaging.Queries;
using Codend.Application.Extensions;
using Codend.Domain.Core.Abstractions;
using FluentValidation;
using static Codend.Application.Core.Errors.ValidationErrors.Common;
using static Codend.Application.Core.Errors.ValidationErrors.Querying;

namespace Codend.Application.Core.Abstractions.Querying;

/// <summary>
/// Abstract validator for queries.
/// </summary>
/// <typeparam name="TQuery">Query type.</typeparam>
/// <typeparam name="TResponse">Response of the query.</typeparam>
/// <typeparam name="TSortColumnSelector">Sort column selector for the query.</typeparam>
/// <typeparam name="TSortEntity">The entity used that will be sorted.</typeparam>
public abstract class AbstractQueryValidator<TQuery, TResponse, TSortColumnSelector, TSortEntity> : AbstractValidator<TQuery>
    where TQuery : class, IQuery<TResponse>, ISortableQuery, IPageableQuery, ITextSearchQuery
    where TSortColumnSelector : ISortColumnSelector<TSortEntity>
    where TSortEntity : class, IEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AbstractQueryValidator{TQuery,TResponse,TSortColumnSelector,TSortEntity}"/> class.
    /// </summary>
    protected AbstractQueryValidator()
    {
        RuleFor(query => query.PageSize)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(IPageableQuery.PageSize)))
            .InclusiveBetween(IPageableQuery.MinPageSize, IPageableQuery.MaxPageSize)
            .WithError(new InvalidPageSize());
        
        RuleFor(query => query.PageIndex)
            .NotEmpty()
            .WithError(new PropertyNullOrEmpty(nameof(IPageableQuery.PageIndex)))
            .GreaterThanOrEqualTo(IPageableQuery.MinPageIndex)
            .WithError(new InvalidPageIndex());

        When(query => query.SortOrder != null, () =>
        {
            RuleFor(x => x.SortOrder)
                .Must(BeValidSortOrder)
                .WithError(new InvalidSortOrder());
        });

        When(query => query.SortColumn != null, () =>
        {
            RuleFor(x => x.SortColumn)
                .Must(x => TSortColumnSelector.SupportedSelectors.Contains(x))
                .WithError(new NotSupportedOrderColumnSelector(TSortColumnSelector.SupportedSelectors));
        });
    }

    private static bool BeValidSortOrder(string? sortOrder)
    {
        return sortOrder?.ToLower() == "asc" || sortOrder?.ToLower() == "desc";
    }
}