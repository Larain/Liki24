using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Liki24.Contracts.Interfaces;

namespace Liki24.BL.Interfaces
{
    public interface IExpressionFactory<TRequest, TResult> where TRequest : ICacheKey
    {
        Expression<Func<TResult, bool>> GetExpression(ICollection<TRequest> request);
        Expression<Func<TResult, bool>> GetExpression(TRequest request);
    }
}