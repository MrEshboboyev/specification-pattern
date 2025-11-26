﻿using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification;

public interface ISpecification<T>
{
    bool IsSatisfiedBy(T entity);
    Task<bool> IsSatisfiedByAsync(T entity, CancellationToken cancellationToken = default);
    Expression<Func<T, bool>> AsExpression();
}
