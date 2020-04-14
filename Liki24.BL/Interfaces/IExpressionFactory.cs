using System;
using System.Linq.Expressions;
using Liki24.Contracts.Interfaces;

namespace Liki24.BL.Interfaces
{
    public interface IExpressionFactory<in TRequest, TResult> where TRequest : ICacheKey<TRequest>
    {
        Expression<Func<TResult, bool>> GetExpression(TRequest request);
    }
}