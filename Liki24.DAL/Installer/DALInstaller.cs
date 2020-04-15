using Liki24.DAL.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Liki24.DAL.Installer
{
    public static class DALInstaller
    {
        public static void AddDalLayer(this IServiceCollection services)
        {
            services.AddTransient<IRepository<DeliveryInterval>, DeliveryIntervalRepository>();
        }
    }
}