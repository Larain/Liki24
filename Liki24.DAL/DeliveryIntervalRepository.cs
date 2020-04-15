using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Liki24.DAL.Models;

namespace Liki24.DAL
{
    public class DeliveryIntervalRepository : IRepository<DeliveryInterval>
    {
        #region seed

        private static readonly List<DeliveryInterval> MemoryRepository = new List<DeliveryInterval>
        {
            new DeliveryInterval
            {
                Id = 0,
                AvailableDaysOfWeek = new[]
                    {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday},
                AvailableFrom = default,
                AvailableTo = TimeSpan.Parse("10:00"),
                Description = "Morning",
                Name = "Morning",
                ExpectedFrom = TimeSpan.Parse("14:00"),
                ExpectedTo = TimeSpan.Parse("18:00"),
                Price = 40,
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                Id = 1,
                AvailableDaysOfWeek = new[]
                    {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday},
                AvailableFrom = default,
                AvailableTo = TimeSpan.Parse("13:00"),
                Description = "Day",
                Name = "Day",
                ExpectedFrom = TimeSpan.Parse("19:00"),
                ExpectedTo = TimeSpan.Parse("23:00"),
                Price = 50,
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                Id = 2,
                AvailableDaysOfWeek = new[] {DayOfWeek.Saturday, DayOfWeek.Sunday},
                AvailableFrom = TimeSpan.Parse("09:15"),
                AvailableTo = TimeSpan.Parse("11:45"),
                Description = "Weekend Morning",
                Name = "Weekend Morning",
                ExpectedFrom = TimeSpan.Parse("14:10"),
                ExpectedTo = TimeSpan.Parse("16:50"),
                Price = 60,
                Type = DeliveryIntervalType.Regular
            },
            new DeliveryInterval
            {
                Id = 3,
                AvailableDaysOfWeek = new[]
                {
                    DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday,
                    DayOfWeek.Friday, DayOfWeek.Saturday, DayOfWeek.Sunday
                },
                AvailableFrom = TimeSpan.Parse("08:00"),
                AvailableTo = TimeSpan.Parse("19:00"),
                Description = "All Day Weekend",
                Name = "All Day Weekend",
                ExpectedFrom = TimeSpan.Parse("11:00"),
                ExpectedTo = TimeSpan.Parse("19:00"),
                Price = 60,
                Type = DeliveryIntervalType.Urgent
            }
        };

        #endregion

        public IQueryable<DeliveryInterval> GetAll()
        {
            return MemoryRepository.AsQueryable();
        }

        public Task<IQueryable<DeliveryInterval>> GetAllAsync()
        {
            // there could be real IQueryable collection
            return Task.FromResult(MemoryRepository.AsQueryable());
        }

        public Task<DeliveryInterval> GetAsync(int id)
        {
            return Task.FromResult(MemoryRepository.FirstOrDefault(d => d.Id == id));
        }

        public Task<DeliveryInterval> AddAsync(DeliveryInterval entity)
        {
            entity.Id = MemoryRepository.Max(d => d.Id) + 1; 
            MemoryRepository.Add(entity);
            return Task.FromResult(entity);
        }

        public Task<bool> UpdateAsync(DeliveryInterval entity)
        {
            var item = MemoryRepository.FirstOrDefault(d => d.Id == entity.Id);
            if (item == null) return Task.FromResult(false);

            var index = MemoryRepository.IndexOf(item);
            MemoryRepository[index] = entity;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var item = MemoryRepository.FirstOrDefault(d => d.Id == id);
            if (item == null) return Task.FromResult(false);

            var index = MemoryRepository.IndexOf(item);
            MemoryRepository.RemoveAt(index);
            return Task.FromResult(true);
        }
    }
}