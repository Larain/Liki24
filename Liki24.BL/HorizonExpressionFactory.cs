using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Liki24.BL.Helpers;
using Liki24.BL.Interfaces;
using Liki24.BL.Models;
using Liki24.Contracts.Models;
using Liki24.DAL.Models;

namespace Liki24.BL
{
    public class HorizonExpressionFactory : IExpressionFactory<GetDeliveryIntervalsForHorizonRequest, DeliveryInterval>
    {
        // quick cache for expression trees
        private static readonly
            ConcurrentDictionary<GetDeliveryIntervalsForHorizonRequest, Expression<Func<DeliveryInterval, bool>>> GetDeliveriesExpressionCache
                = new ConcurrentDictionary<GetDeliveryIntervalsForHorizonRequest, Expression<Func<DeliveryInterval, bool>>>();

        public Expression<Func<DeliveryInterval, bool>> GetExpression(GetDeliveryIntervalsForHorizonRequest request)
        {
            if (!GetDeliveriesExpressionCache.TryGetValue(request, out var finalExpression))
            {
                var searchRequests = CreateSearchRequests(request);
                var searchExpressions = searchRequests.Select(CreateExpression).ToList();

                finalExpression = searchExpressions.Skip(1).Aggregate(searchExpressions.First(), (a, qs) => a.Or(qs));
                GetDeliveriesExpressionCache[request] = finalExpression;
            }

            return finalExpression;
        }

        private static IEnumerable<SearchRequest> CreateSearchRequests(GetDeliveryIntervalsForHorizonRequest request)
        {
            var horizon = request.Horizon;
            var startDate = request.CurrentDate;
            var searchRequests = new List<SearchRequest>(horizon + 1)
            {
                // add search for today
                new SearchRequest
                {
                    DayOfWeek = startDate.DayOfWeek,
                    LookFrom = startDate.TimeOfDay,
                    LookTo = TimeSpan.FromHours(24)
                }
            };

            // add search for all next days
            var currentDay = startDate.DayOfWeek;
            for (var i = 1; i <= horizon; i++)
            {
                currentDay = currentDay.GetNextDay();
                searchRequests.Add(new SearchRequest { DayOfWeek = currentDay, LookFrom = TimeSpan.Zero, LookTo = TimeSpan.FromHours(24) });
            }

            return searchRequests;
        }

        private static Expression<Func<DeliveryInterval, bool>> CreateExpression(SearchRequest searchRequest)
        {
            return di => di.AvailableDaysOfWeek.Contains(searchRequest.DayOfWeek)
                         && searchRequest.LookFrom <= di.AvailableFrom
                         && searchRequest.LookTo >= di.AvailableTo;
        }
    }
}