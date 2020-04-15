using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Liki24.BL.Helpers;
using Liki24.BL.Interfaces;
using Liki24.Contracts.Models;
using Liki24.DAL.Models;

namespace Liki24.BL
{
    public class HorizonExpressionFactory : IExpressionFactory<SearchRequest, DeliveryInterval>
    {
        // quick cache for expression trees
        private static readonly
            ConcurrentDictionary<string, Expression<Func<DeliveryInterval, bool>>> GetDeliveriesExpressionCache
                = new ConcurrentDictionary<string, Expression<Func<DeliveryInterval, bool>>>();

        public Expression<Func<DeliveryInterval, bool>> GetExpression(ICollection<SearchRequest> requests)
        {
            var cacheKay = string.Join(",", requests.Select(x => x.Key));
            if (!GetDeliveriesExpressionCache.TryGetValue(cacheKay, out var finalExpression))
            {
                var searchExpressions = requests.Select(CreateExpression).ToList();

                finalExpression = searchExpressions.Skip(1).Aggregate(searchExpressions.First(), (a, qs) => a.Or(qs));
                GetDeliveriesExpressionCache[cacheKay] = finalExpression;
            }

            return finalExpression;
        }

        public Expression<Func<DeliveryInterval, bool>> GetExpression(SearchRequest requests)
        {
            var cacheKay = requests.Key;
            if (!GetDeliveriesExpressionCache.TryGetValue(cacheKay, out var finalExpression))
            {
                finalExpression = CreateExpression(requests);
                GetDeliveriesExpressionCache[cacheKay] = finalExpression;
            }

            return finalExpression;
        }

        private static Expression<Func<DeliveryInterval, bool>> CreateExpression(SearchRequest searchRequest)
        {
            Expression<Func<DeliveryInterval, bool>> expression = di => di.AvailableDaysOfWeek.Contains(searchRequest.DayOfWeek);
            if (searchRequest.LookFrom.HasValue)
            {
                expression = expression.And(di => di.AvailableTo >= searchRequest.LookFrom);
            }
            if (searchRequest.LookTo.HasValue)
            {
                expression = expression.And(di => di.AvailableFrom <= searchRequest.LookTo);
            }

            return expression;
        }
    }
}