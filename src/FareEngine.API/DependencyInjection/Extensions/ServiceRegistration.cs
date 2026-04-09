using FareEngine.Application.FarePolicies;
using FareEngine.Application.Modifications;
using FareEngine.Application.SoldProducts;
using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;
using FareEngine.Infrastructure.FarePolicies;
using FareEngine.Infrastructure.Modifications;
using FareEngine.Infrastructure.Persistence;
using FareEngine.Infrastructure.Persistence.Repositories.FarePolicies;
using FareEngine.Infrastructure.Persistence.Repositories.Modifications;
using FareEngine.Infrastructure.Persistence.Repositories.SoldProducts;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.API.DependencyInjection.Extensions;

public static class ServiceRegistration
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<SoldProductManager>();
        
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IModificationAppService, ModificationAppService>();
        services.AddScoped<IFarePolicyAppService, FarePolicyAppService>();
        services.AddScoped<ISoldProductAppService, SoldProductAppService>();
        
        services.AddScoped<IBillService, BillService>();
        
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString)
        );

        services.AddScoped<ISoldProductRepository, SoldProductRepository>();
        services.AddScoped<IFarePolicyRepository, FarePolicyRepository>();
        services.AddScoped<IModificationRepository, ModificationRepository>();

        services.AddScoped<ISoldProductReadRepository, SoldProductReadRepository>();
        services.AddScoped<IModificationReadRepository, ModificationReadRepository>();
        services.AddScoped<IFarePolicyReadRepository, FarePolicyReadRepository>();
        
        services.AddScoped<FlatRateFarePolicyCalculator>();
        services.AddScoped<DistanceBasedFarePolicyCalculator>();
        services.AddScoped<ZoneBasedFarePolicyCalculator>();

        services.AddScoped<FirstClassModificationCalculator>();
        services.AddScoped<SeniorDiscountModificationCalculator>();

        services.AddScoped<IFarePolicyCalculatorFactory, FarePolicyCalculatorFactory>();
        services.AddScoped<IModificationCalculatorFactory, ModificationCalculatorFactory>();

        return services;
    }
}