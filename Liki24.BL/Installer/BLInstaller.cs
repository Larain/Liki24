﻿using AutoMapper;
using Liki24.BL.Base;
using Liki24.BL.Interfaces;
using Liki24.Contracts.Interfaces;
using Liki24.Contracts.Models;
using Liki24.DAL.Installer;
using Liki24.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Liki24.BL.Installer
{
    public static class BLInstaller
    {
        public static void AddBlLayer(this IServiceCollection services)
        {
            services.AddDalLayer();
            services.AddTransient<IManager<DeliveryIntervalDto>, DeliveryIntervalManager>();
            services.AddTransient<IExpressionFactory<SearchRequest, DeliveryInterval>, HorizonExpressionFactory>();
            services.AddTransient<IDeliveriesService, DeliveriesService>();
            services.AddTransient<ISearchRequestFactory, SearchRequestFactory>();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<DeliveryIntervalMapperProfile>();
            });
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}