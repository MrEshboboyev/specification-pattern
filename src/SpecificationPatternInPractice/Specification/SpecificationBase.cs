﻿using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification;

public abstract class SpecificationBase<T> : ISpecification<T>
{
    public abstract Expression<Func<T, bool>> AsExpression();

    public bool IsSatisfiedBy(T entity)
    {
        var predicate = AsExpression().Compile();
        return predicate(entity);
    }

    public virtual async Task<bool> IsSatisfiedByAsync(
        T entity,
        CancellationToken cancellationToken = default)
    {
        return await Task.Run(() => IsSatisfiedBy(entity), cancellationToken);
    }
}
