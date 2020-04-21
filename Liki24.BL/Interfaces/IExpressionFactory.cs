using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Liki24.Contracts.Interfaces;
using Liki24.Contracts.Models;
using Liki24.DAL.Models;

namespace Liki24.BL.Interfaces
{
    public interface IExpressionFactory<TRequest, TResult> where TRequest : ICacheKey
    {
        Expression<Func<TResult, bool>> GetExpression(ICollection<TRequest> request);
        Func<DeliveryInterval, bool> GetCompiledExpression(SearchRequest requests);
    }
}