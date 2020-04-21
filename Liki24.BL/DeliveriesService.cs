using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Liki24.BL.Interfaces;
using Liki24.Contracts.Interfaces;
using Liki24.Contracts.Models;
using Liki24.DAL;
using Liki24.DAL.Models;
using DeliveryIntervalType = Liki24.DAL.Models.DeliveryIntervalType;

namespace Liki24.BL
{
    public class DeliveriesService : IDeliveriesService
    {
        private readonly IMapper _mapper;
        private readonly ISearchRequestFactory _searchRequestFactory;
        private readonly IRepository<DeliveryInterval> _repository;
        private readonly IExpressionFactory<SearchRequest, DeliveryInterval> _horizonExpressionFactory;

        public DeliveriesService(IRepository<DeliveryInterval> repository, IMapper mapper, ISearchRequestFactory searchRequestFactory,
            IExpressionFactory<SearchRequest, DeliveryInterval> horizonExpressionFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _searchRequestFactory = searchRequestFactory;
            _horizonExpressionFactory = horizonExpressionFactory;
        }

        public ICollection<ClientDeliveryInterval> GetDeliveriesForHorizon(GetDeliveryIntervalsForHorizonRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var searchRequests = _searchRequestFactory.CreateSearchRequests(request.Horizon, request.CurrentDate);
            var dbData = GetIntervalsFromDb(searchRequests);
            return FilterByDays(searchRequests, dbData, request.CurrentDate);
        }

        private List<ClientDeliveryInterval> FilterByDays(ICollection<SearchRequest> requests, ICollection<DeliveryInterval> dbData, DateTime startDate)
        {
            var resultList = new List<ClientDeliveryInterval>(dbData.Count);
            var weekCounter = 1;
            var daysCounter = (int)startDate.DayOfWeek;
            foreach (var searchRequest in requests)
            {
                var intervals = dbData.Where(_horizonExpressionFactory.GetCompiledExpression(searchRequest));
                var counter = weekCounter;
                var clientIntervals = intervals.Select(ci =>
                {
                    var interval = _mapper.Map<ClientDeliveryInterval>(ci);
                    interval.DayOfWeek = new Value((int) searchRequest.DayOfWeek, searchRequest.DayOfWeek.ToString());
                    interval.WeekNumber = counter;
                    interval.Available = IsTimeAvailable(ci, startDate, searchRequest.DayOfWeek, interval.AvailabilityDeltaHours, counter);
                    return interval;
                });
                resultList.AddRange(clientIntervals);
                if (daysCounter++ % 7 == 0) weekCounter++;
            }

            return resultList.OrderBy(x => x.WeekNumber).ThenBy(x => (DayOfWeek)x.DayOfWeek.Id).ThenBy(x => x.AvailableTo).ToList();
        }

        /// I suggest that real project would use EF to access DB
        private ICollection<DeliveryInterval> GetIntervalsFromDb(ICollection<SearchRequest> requests)
        {
            var uniqueRequests = new HashSet<SearchRequest>(requests);
            // there would be real IQueryable<DeliveryInterval>
            var lambda = _horizonExpressionFactory.GetExpression(uniqueRequests);
            return _repository.GetAll().Where(lambda).ToList(); // we could use .AsNoTracking() here
        }

        private static bool IsTimeAvailable(DeliveryInterval interval, DateTime startDate, DayOfWeek currentDayOfWeek, uint? delta, int weekNumber)
        {
            // urgent delivery is available only for today
            if (interval.Type == DeliveryIntervalType.Urgent)
            {
                return CheckTime(interval, startDate, currentDayOfWeek);
            }

            var result = CheckTime(interval, startDate, currentDayOfWeek);
            if (result) return true;

            if (!delta.HasValue) return false;

            var intervalDate = GetIntervalDate(startDate, currentDayOfWeek, weekNumber, interval);
            return startDate >= intervalDate.AddHours((int)delta * -1);
        }

        private static bool CheckTime(DeliveryInterval interval, DateTime startDate, DayOfWeek currentDayOfWeek)
        {
            return startDate.DayOfWeek == currentDayOfWeek && startDate.TimeOfDay >= interval.AvailableFrom && startDate.TimeOfDay <= interval.AvailableTo;
        }

        private static DateTime GetIntervalDate(DateTime startDate, DayOfWeek currentDayOfWeek, int weekNumber, DeliveryInterval interval)
        {
            // calculate how much days pass from startDate
            var currentDaysPass = (int)currentDayOfWeek - (int)startDate.DayOfWeek +  (weekNumber - 1) * 7;
            var date = startDate.AddDays(currentDaysPass);
            return new DateTime(date.Year, date.Month, date.Day, interval.AvailableFrom.Hours, interval.AvailableFrom.Minutes, 0);
        }
    }
}