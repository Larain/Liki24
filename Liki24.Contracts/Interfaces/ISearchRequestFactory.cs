using System;
using System.Collections.Generic;
using Liki24.Contracts.Models;

namespace Liki24.Contracts.Interfaces
{
    public interface ISearchRequestFactory
    {
        ICollection<SearchRequest> CreateSearchRequests(uint horizon, DateTime startDate);
    }
}