using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoMapper;
using Liki24.BL;
using Liki24.BL.Base;
using Liki24.BL.Interfaces;
using Liki24.Contracts.Interfaces;
using Liki24.Contracts.Models;
using Liki24.DAL;
using Liki24.DAL.Models;
using Moq;
using NUnit.Framework;
using DeliveryIntervalType = Liki24.DAL.Models.DeliveryIntervalType;

namespace Liki24.Tests
{
    public class Tests
    {
        private IMapper _mapper;
        private List<DeliveryInterval> _memoryRepository;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DeliveryIntervalMapperProfile>();
            });
            _mapper = config.CreateMapper();

            #region seed

            _memoryRepository = new List<DeliveryInterval>
            {
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new[]
                    {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday},
                AvailableFrom = default,
                AvailableTo = TimeSpan.Parse("10:00"),
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new[]
                    {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday},
                AvailableFrom = default,
                AvailableTo = TimeSpan.Parse("13:00"),
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new[]
                {
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                    DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
                },
                AvailableFrom = TimeSpan.Parse("00:00"),
                AvailableTo = TimeSpan.Parse("00:01"),
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new[]
                {
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                    DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
                },
                AvailableFrom = TimeSpan.Parse("08:00"),
                AvailableTo = TimeSpan.Parse("19:00"),
                Type = DeliveryIntervalType.Urgent
            },
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new[]
                {
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                    DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
                },
                AvailableFrom = TimeSpan.Parse("23:50"),
                AvailableTo = TimeSpan.Parse("23:55"),
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new[] {DayOfWeek.Saturday, DayOfWeek.Sunday},
                AvailableFrom = TimeSpan.Parse("09:15"),
                AvailableTo = TimeSpan.Parse("11:45"),
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                AvailableDaysOfWeek = new DayOfWeek[0],
                AvailableFrom = TimeSpan.Parse("14:59"),
                AvailableTo = TimeSpan.Parse("16:00"),
                Type = DeliveryIntervalType.Urgent
            },
        };

            #endregion
        }

        [Test]
        public void Assert_configuration_is_valid()
        {
            var repository = new Mock<IRepository<DeliveryInterval>>();
            var mapper = new Mock<IMapper>();

            var searchRequestFactory = new Mock<ISearchRequestFactory>();
            searchRequestFactory.Setup(x => x.CreateSearchRequests(It.IsAny<uint>(), It.IsAny<DateTime>()))
                .Returns(new List<SearchRequest>());

            var expressionFactory = new Mock<IExpressionFactory<SearchRequest, DeliveryInterval>>();
            expressionFactory.Setup(ef => ef.GetExpression(It.IsAny<ICollection<SearchRequest>>())).Returns(x => true);
            expressionFactory.Setup(ef => ef.GetCompiledExpression(It.IsAny<SearchRequest>())).Returns(x => true);
            
            var deliveriesService = new DeliveriesService(repository.Object, mapper.Object, searchRequestFactory.Object, expressionFactory.Object);
            deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest {CurrentDate = DateTime.Now, Horizon = 5});
            Assert.Pass();
        }

        [Test]
        [TestCase("14.04.2020 00:00:00", 5u)] // tuesday
        [TestCase("14.04.2020 04:25:00", 4u)] // tuesday
        [TestCase("14.04.2020 08:31:00", 4u)] // tuesday
        [TestCase("14.04.2020 13:26:00", 2u)] // tuesday
        [TestCase("14.04.2020 16:07:00", 2u)] // tuesday
        [TestCase("14.04.2020 20:25:00", 1u)] // tuesday
        [TestCase("14.04.2020 23:59:00", 0u)] // tuesday
        public void Assert_return_expected_items_count_for_different_time(string currentDate, uint expectedResult)
        {
            var repository = new Mock<IRepository<DeliveryInterval>>();
            repository.Setup(r => r.GetAll()).Returns(_memoryRepository.AsQueryable());

            var expressionFactory = new HorizonExpressionFactory();
            var searchRequestFactory = new SearchRequestFactory();
            var deliveriesService = new DeliveriesService(repository.Object, _mapper, searchRequestFactory, expressionFactory);

            var result = deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest
                {CurrentDate = DateTime.Parse(currentDate, new CultureInfo("ru-RU")), Horizon = 0 });
            Assert.AreEqual(expectedResult, result.Count);
        }

        [Test]
        [TestCase(0u, 2u)] // tuesday
        [TestCase(1u, 6u)] // wednesday
        [TestCase(2u, 11u)] // thursday
        [TestCase(3u, 16u)] // friday (hooray!)
        [TestCase(4u, 20u)] // saturday
        [TestCase(5u, 24u)] // sunday
        [TestCase(6u, 29u)] // monday
        [TestCase(7u, 34u)] // tuesday
        public void Assert_return_expected_items_count_different_horizon(uint horizon, uint expectedResult)
        {
            var repository = new Mock<IRepository<DeliveryInterval>>();
            repository.Setup(r => r.GetAll()).Returns(_memoryRepository.AsQueryable());

            var expressionFactory = new HorizonExpressionFactory();
            var searchRequestFactory = new SearchRequestFactory();
            var deliveriesService = new DeliveriesService(repository.Object, _mapper, searchRequestFactory, expressionFactory);

            var result = deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest
                {CurrentDate = DateTime.Parse("14.04.2020 13:26:00", new CultureInfo("ru-RU")), Horizon = horizon });
            Assert.AreEqual(expectedResult, result.Count);
        }

        [Test]
        [TestCase(DeliveryIntervalType.Urgent, "08:12", "11:24", DayOfWeek.Tuesday, "14.04.2020 11:26:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "16:45", "18:32", DayOfWeek.Tuesday, "14.04.2020 17:26:00", 1)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "17:11", "23:23", DayOfWeek.Sunday, "14.04.2020 14:26:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "03:22", "20:32", DayOfWeek.Friday, "18.04.2020 20:26:00", 0)] // saturday
        [TestCase(DeliveryIntervalType.Regular, "14:32", "17:22", DayOfWeek.Saturday, "18.04.2020 14:26:00", 0)] // saturday
        [TestCase(DeliveryIntervalType.Urgent, "11:11", "20:53", DayOfWeek.Saturday, "18.04.2020 18:26:00", 1)] // saturday
        [TestCase(DeliveryIntervalType.Regular, "15:22", "17:32", DayOfWeek.Thursday, "17.04.2020 16:26:00", 0)] // friday
        [TestCase(DeliveryIntervalType.Urgent, "03:32", "05:53", DayOfWeek.Tuesday, "17.04.2020 13:26:00", 0)] // friday
        [TestCase(DeliveryIntervalType.Regular, "00:00", "08:12", DayOfWeek.Friday, "17.04.2020 10:26:00", 0)] // friday
        [TestCase(DeliveryIntervalType.Regular, "00:00", "08:12", DayOfWeek.Monday, "17.04.2020 06:26:00", 0)] // friday
        public void Assert_return_expected_available_items_count(DeliveryIntervalType type, string from, string to, DayOfWeek day, string startDate, int expectedResult)
        {
            var repository = new Mock<IRepository<DeliveryInterval>>();

            var testData = new List<DeliveryInterval>
            {
                new DeliveryInterval
                {
                    Type = type,
                    AvailableFrom = TimeSpan.Parse(from),
                    AvailableTo = TimeSpan.Parse(to),
                    AvailableDaysOfWeek = new List<DayOfWeek> {day},
                }
            };

            repository.Setup(r => r.GetAll()).Returns(testData.AsQueryable);

            var expressionFactory = new HorizonExpressionFactory();
            var searchRequestFactory = new SearchRequestFactory();
            var deliveriesService = new DeliveriesService(repository.Object, _mapper, searchRequestFactory, expressionFactory);

            var result = deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest
                {CurrentDate = DateTime.Parse(startDate, new CultureInfo("ru-RU")), Horizon = 0 });
            Assert.AreEqual(expectedResult, result.Count(x => x.Available));
        }

        [Test]
        [TestCase(DeliveryIntervalType.Urgent, "09:00", "15:00", DayOfWeek.Wednesday, 13u, "14.04.2020 22:00:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "09:00", "15:00", DayOfWeek.Wednesday, 13u, "14.04.2020 22:00:00", 1)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "09:00", "15:00", DayOfWeek.Wednesday, 13u, "13.04.2020 22:00:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "09:00", "15:00", DayOfWeek.Wednesday, 13u, "14.04.2020 11:00:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Urgent, "09:00", "15:00", DayOfWeek.Wednesday, 9u, "14.04.2020 15:26:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "09:00", "15:00", DayOfWeek.Wednesday, 9u, "14.04.2020 15:26:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Urgent, "09:00", "15:00", DayOfWeek.Wednesday, 2u, "14.04.2020 15:26:00", 0)] // tuesday
        [TestCase(DeliveryIntervalType.Regular, "09:00", "15:00", DayOfWeek.Wednesday, 2u, "14.04.2020 15:26:00", 0)] // tuesday
        public void Assert_return_expected_available_items_count_with_delta(DeliveryIntervalType type, string from, string to, DayOfWeek day, uint delta, string startDate, int expectedResult)
        {
            var repository = new Mock<IRepository<DeliveryInterval>>();

            var testData = new List<DeliveryInterval>
            {
                new DeliveryInterval
                {
                    Type = type,
                    AvailableFrom = TimeSpan.Parse(from),
                    AvailableTo = TimeSpan.Parse(to),
                    AvailableDaysOfWeek = new List<DayOfWeek> {day},
                    AvailabilityDeltaHours = delta
                }
            };

            repository.Setup(r => r.GetAll()).Returns(testData.AsQueryable);

            var expressionFactory = new HorizonExpressionFactory();
            var searchRequestFactory = new SearchRequestFactory();
            var deliveriesService = new DeliveriesService(repository.Object, _mapper, searchRequestFactory, expressionFactory);

            var result = deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest
                {CurrentDate = DateTime.Parse(startDate, new CultureInfo("ru-RU")), Horizon = 0 });
            Assert.AreEqual(expectedResult, result.Count(x => x.Available));
        }

        // todo: test exceptions
    }
}