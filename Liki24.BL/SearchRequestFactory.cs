using System;
using System.Collections.Generic;
using System.Linq;
using Liki24.BL.Helpers;
using Liki24.Contracts.Interfaces;
using Liki24.Contracts.Models;

namespace Liki24.BL
{
    public class SearchRequestFactory : ISearchRequestFactory
    {
        public ICollection<SearchRequest> CreateSearchRequests(uint horizon, DateTime startDate)
        {
            var searchRequests = new List<SearchRequest>((int) horizon + 2);
            var firstDaySearch = new SearchRequest
            {
                DayOfWeek = startDate.DayOfWeek,
                LookFrom = startDate.TimeOfDay,
            };
            var deltaDaySearch = new SearchRequest
            {
                DayOfWeek = startDate.DayOfWeek.GetNextDay(),
                HasDelta = true,
            };
            // add search for today
            searchRequests.Add(firstDaySearch);
            // add search for next day that have delta
            searchRequests.Add(deltaDaySearch);

            // add search for all next days
            var currentDay = startDate.DayOfWeek;
            for (var i = 1; i <= horizon; i++)
            {
                currentDay = currentDay.GetNextDay();
                searchRequests.Add(new SearchRequest { DayOfWeek = currentDay });
            }

            if (horizon > 0)
            {
                var lastDaySearch = searchRequests.Last();
                lastDaySearch.LookTo = startDate.TimeOfDay;
            }

            return searchRequests;
        }
    }
}