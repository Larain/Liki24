using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Liki24.BL.Helpers;
using Liki24.BL.Interfaces;
using Liki24.Contracts.Models;
using Liki24.DAL;
using Liki24.DAL.Models;
using DeliveryIntervalType = Liki24.DAL.Models.DeliveryIntervalType;

namespace Liki24.BL
{
    public class DeliveriesCalculator : IDeliveriesCalculator
    {
        private readonly IMapper _mapper;
        private readonly IRepository<DeliveryInterval> _repository;
        private readonly IExpressionFactory<GetDeliveryIntervalsForHorizonRequest,DeliveryInterval> _horizonExpressionFactory;

        public DeliveriesCalculator(IRepository<DeliveryInterval> repository, IMapper mapper,
            IExpressionFactory<GetDeliveryIntervalsForHorizonRequest, DeliveryInterval> horizonExpressionFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _horizonExpressionFactory = horizonExpressionFactory;
        }

        public ICollection<ClientDeliveryInterval> GetDeliveriesForHorizon(GetDeliveryIntervalsForHorizonRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = GetForEntityFramework(request);
            var resultSet = result.SelectMany(x => x.AvailableDaysOfWeek.Select(d =>
            {
                var interval = _mapper.Map<ClientDeliveryInterval>(x);
                interval.DayOfWeek = new Value((int)d, d.ToString());
                interval.Available = IsTimeAvailable(x, request.CurrentDate, d);
                return interval;
            })).OrderBy(x => (DayOfWeek)x.DayOfWeek.Id).ThenBy(x => x.AvailableTo).ToList();
            return resultSet;
        }

        /// I suggest that real project would use EF to access DB
        private ICollection<DeliveryInterval> GetForEntityFramework(GetDeliveryIntervalsForHorizonRequest request)
        {
            // there would be real IQueryable<DeliveryIntervalDto>
            var lambda = _horizonExpressionFactory.GetExpression(request);
            var result = _repository.GetAll().Where(lambda).ToList();

            // set available days
            var currentDay = request.CurrentDate.DayOfWeek;
            var days = new List<DayOfWeek> { currentDay };

            for (var i = 0; i < request.Horizon; i++)
            {
                currentDay = currentDay.GetNextDay();
                days.Add(currentDay);
            }

            result.ForEach(x => x.AvailableDaysOfWeek = x.AvailableDaysOfWeek.Intersect(days).ToList());
            return result;
        }

        private static bool IsTimeAvailable(DeliveryInterval interval, DateTime dateLookFrom, DayOfWeek currentDayOfWeek)
        {
            if (dateLookFrom.DayOfWeek != currentDayOfWeek)
            {
                // urgent delivery is available only for today
                // regular delivery for all next days is available (?)
                return interval.Type != DeliveryIntervalType.Urgent;
            }

            return dateLookFrom.TimeOfDay >= interval.AvailableFrom && dateLookFrom.TimeOfDay <= interval.AvailableTo;
        }
    }
}