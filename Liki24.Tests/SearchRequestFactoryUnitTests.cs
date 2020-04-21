using System;
using System.Globalization;
using System.Linq;
using Liki24.BL;
using NUnit.Framework;

namespace Liki24.Tests
{
    public class SearchRequestFactoryUnitTests
    {
        [Test]
        [TestCase(0u, "15.04.2020 21:22:00")]
        [TestCase(1u, "14.04.2020 21:22:00")]
        [TestCase(2u, "11.01.2020 21:22:00")]
        [TestCase(5u, "22.04.2020 21:22:00")]
        [TestCase(10u, "15.04.2020 21:22:00")]
        [TestCase(15u, "15.04.2020 21:22:00")]
        [TestCase(40u, "15.04.2020 21:22:00")]
        [TestCase(99u, "15.04.2020 21:22:00")]
        public void Should_create_N_requests(uint horizon, string startDate)
        {
            var factory = new SearchRequestFactory();
            var requests = factory.CreateSearchRequests(horizon, DateTime.Parse(startDate, new CultureInfo("ru-RU")));
            const int additionalDeltaSearchRequestCount = 1;
            const int todaySearchRequestCount = 1;

            Assert.AreEqual(horizon + additionalDeltaSearchRequestCount + todaySearchRequestCount, requests.Count);
        }

        [Test]
        [TestCase(0u, "15.04.2020 21:22:00")]
        [TestCase(1u, "15.04.2020 21:22:00")]
        [TestCase(2u, "15.04.2020 21:22:00")]
        [TestCase(5u, "15.04.2020 21:22:00")]
        [TestCase(10u, "15.04.2020 21:22:00")]
        [TestCase(15u, "15.04.2020 21:22:00")]
        [TestCase(40u, "15.04.2020 21:22:00")]
        [TestCase(99u, "15.04.2020 21:22:00")]
        public void First_should_have_look_from(uint horizon, string startDate)
        {
            var factory = new SearchRequestFactory();
            var requests = factory.CreateSearchRequests(horizon, DateTime.Parse(startDate, new CultureInfo("ru-RU")));
            Assert.IsNotNull(requests.First().LookFrom);
        }

        [Test]
        [TestCase(1u, "15.04.2020 21:22:00")]
        [TestCase(2u, "15.04.2020 21:22:00")]
        [TestCase(5u, "15.04.2020 21:22:00")]
        [TestCase(10u, "15.04.2020 21:22:00")]
        [TestCase(15u, "15.04.2020 21:22:00")]
        [TestCase(40u, "15.04.2020 21:22:00")]
        [TestCase(99u, "15.04.2020 21:22:00")]
        public void Last_should_have_look_to(uint horizon, string startDate)
        {
            var factory = new SearchRequestFactory();
            var requests = factory.CreateSearchRequests(horizon, DateTime.Parse(startDate, new CultureInfo("ru-RU")));
            if (horizon > 0)
            {
                Assert.IsNotNull(requests.Last().LookTo);
            }
        }

        [Test]
        [TestCase(1u, "15.04.2020 21:22:00")]
        [TestCase(2u, "15.04.2020 21:22:00")]
        [TestCase(5u, "15.04.2020 21:22:00")]
        [TestCase(10u, "15.04.2020 21:22:00")]
        [TestCase(15u, "15.04.2020 21:22:00")]
        [TestCase(40u, "15.04.2020 21:22:00")]
        [TestCase(99u, "15.04.2020 21:22:00")]
        public void Second_should_have_delta(uint horizon, string startDate)
        {
            var factory = new SearchRequestFactory();
            var requests = factory.CreateSearchRequests(horizon, DateTime.Parse(startDate, new CultureInfo("ru-RU")));
            Assert.IsNotNull(requests.Skip(1).Take(1).Single().HasDelta);
        }
    }
}