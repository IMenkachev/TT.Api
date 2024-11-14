using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TT.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDb<TDbContext, VDbContext>(this IServiceCollection services)
            where VDbContext : TDbContext
            where TDbContext : DbContext
            => services.AddScoped<TDbContext, VDbContext>();

        //public static IServiceCollection AddAutoMapperProfile(this IServiceCollection services, Assembly assembly)
        //    => services.AddAutoMapper(new MappingProfile(assembly).GetType());
    }
}
