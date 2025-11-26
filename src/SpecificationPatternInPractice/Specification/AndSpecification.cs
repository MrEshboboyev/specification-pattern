﻿using System.Linq.Expressions;

namespace SpecificationPatternInPractice.Specification;

public class AndSpecification<T>(
    ISpecification<T> left,
    ISpecification<T> right
) : SpecificationBase<T>
{
    public override Expression<Func<T, bool>> AsExpression()
    {
        var leftExpr = left.AsExpression();
        var rightExpr = right.AsExpression();
        
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(leftExpr, parameter),
            Expression.Invoke(rightExpr, parameter)
        );
        
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }

    public override async Task<bool> IsSatisfiedByAsync(
        T entity, 
        CancellationToken cancellationToken = default)
    {
        var leftResult = await left.IsSatisfiedByAsync(entity, cancellationToken);
        if (!leftResult) return false; // Short-circuit evaluation
        return await right.IsSatisfiedByAsync(entity, cancellationToken);
    }
}
