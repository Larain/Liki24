using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Liki24.BL;
using Liki24.BL.Base;
using Liki24.BL.Interfaces;
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

            var expressionFactory = new Mock<IExpressionFactory<GetDeliveryIntervalsForHorizonRequest, DeliveryInterval>>();
            expressionFactory.Setup(ef => ef.GetExpression(It.IsAny<GetDeliveryIntervalsForHorizonRequest>())).Returns(x => true);

            var deliveriesService = new DeliveriesService(repository.Object, mapper.Object, expressionFactory.Object);
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
            var deliveriesService = new DeliveriesService(repository.Object, _mapper, expressionFactory);

            var result = deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest
                {CurrentDate = DateTime.Parse(currentDate), Horizon = 0 });
            Assert.AreEqual(expectedResult, result.Count);
        }

        [Test]
        [TestCase(0u, 5u)] // tuesday
        [TestCase(1u, 4u)] // wednesday
        [TestCase(2u, 4u)] // thursday
        [TestCase(3u, 2u)] // friday (hooray!)
        [TestCase(4u, 2u)] // saturday
        [TestCase(5u, 1u)] // sunday
        [TestCase(6u, 0u)] // monday
        [TestCase(7u, 0u)] // wednesday
        public void Assert_return_expected_items_count_different_horizon(uint horizon, uint expectedResult)
        {
            var repository = new Mock<IRepository<DeliveryInterval>>();
            repository.Setup(r => r.GetAll()).Returns(_memoryRepository.AsQueryable());

            var expressionFactory = new HorizonExpressionFactory();
            var deliveriesService = new DeliveriesService(repository.Object, _mapper, expressionFactory);

            var result = deliveriesService.GetDeliveriesForHorizon(new GetDeliveryIntervalsForHorizonRequest
                {CurrentDate = DateTime.Parse("14.04.2020 13:26:00"), Horizon = horizon });
            Assert.AreEqual(expectedResult, result.Count);
        }

        // todo: test exceptions
    }
}