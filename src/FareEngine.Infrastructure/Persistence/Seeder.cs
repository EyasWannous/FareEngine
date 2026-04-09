using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;
using Microsoft.Extensions.Logging;

namespace FareEngine.Infrastructure.Persistence;

public static class Seeder
{
    public static async Task SeedAsync(AppDbContext context, SoldProductManager soldProductManager, ILogger logger)
    {
        if (context.FarePolicies.Any() || context.Modifications.Any() || context.SoldProducts.Any())
        {
            logger.LogInformation("Database already seeded. Skipping.");
            return;
        }
        
        logger.LogInformation("Seeding database...");

        // ── Fare Policies ──────────────────────────────────────────────

        var flatRatePolicy = new FlatRateFarePolicy(
            id: Guid.Parse("00000000-0000-0000-0000-000000000001"),
            name: "Standard Daily Pass",
            flatAmount: 8.00m);

        var distanceBasedPolicy = new DistanceBasedFarePolicy(
            id: Guid.Parse("00000000-0000-0000-0000-000000000002"),
            name: "Standard Distance Rate",
            ratePerKm: 0.10m);

        var zoneBasedPolicyZone1 = new ZoneBasedFarePolicy(
            id: Guid.Parse("00000000-0000-0000-0000-000000000003"),
            name: "Marine Zone 1",
            zoneNumber: 1,
            zonePrice: 5.00m);

        var zoneBasedPolicyZone2 = new ZoneBasedFarePolicy(
            id: Guid.Parse("00000000-0000-0000-0000-000000000004"),
            name: "Marine Zone 2",
            zoneNumber: 2,
            zonePrice: 10.00m);

        var zoneBasedPolicyZone3 = new ZoneBasedFarePolicy(
            id: Guid.Parse("00000000-0000-0000-0000-000000000005"),
            name: "Marine Zone 3",
            zoneNumber: 3,
            zonePrice: 15.00m);

        await context.FarePolicies.AddRangeAsync(
            flatRatePolicy,
            distanceBasedPolicy,
            zoneBasedPolicyZone1,
            zoneBasedPolicyZone2,
            zoneBasedPolicyZone3);
        
        await context.SaveChangesAsync();

        // ── Modifications ──────────────────────────────────────────────

        var firstClassModification = new FirstClassModification(
            id: Guid.Parse("00000000-0000-0000-0000-000000000010"),
            name: "First Class",
            surcharge: 15.00m);

        var seniorDiscountModification = new SeniorDiscountModification(
            id: Guid.Parse("00000000-0000-0000-0000-000000000011"),
            name: "Senior Discount",
            discountPercentage: 0.20m);

        await context.Modifications.AddRangeAsync(
            firstClassModification,
            seniorDiscountModification);

        await context.SaveChangesAsync();

        // ── Sold Products ──────────────────────────────────────────────

        // Daily passes
        var dailyPass1 = await soldProductManager.CreateDailyPassAsync(
            flatRatePolicy.Id, []);

        var dailyPass2 = await soldProductManager.CreateDailyPassAsync(
            flatRatePolicy.Id, [firstClassModification.Id]);

        var dailyPass3 = await soldProductManager.CreateDailyPassAsync(
            flatRatePolicy.Id, [seniorDiscountModification.Id]);

        var dailyPass4 = await soldProductManager.CreateDailyPassAsync(
            flatRatePolicy.Id, [firstClassModification.Id, seniorDiscountModification.Id]);

        // Hybrid trips
        var hybridTrip1 = await soldProductManager.CreateHybridTripAsync(
            distanceInKm: 50m, distanceBasedPolicy.Id, zoneBasedPolicyZone1.Id, []);

        var hybridTrip2 = await soldProductManager.CreateHybridTripAsync(
            distanceInKm: 80m, distanceBasedPolicy.Id, zoneBasedPolicyZone2.Id,
            [firstClassModification.Id]);

        var hybridTrip3 = await soldProductManager.CreateHybridTripAsync(
            distanceInKm: 120m, distanceBasedPolicy.Id, zoneBasedPolicyZone3.Id,
            [seniorDiscountModification.Id]);

        var hybridTrip4 = await soldProductManager.CreateHybridTripAsync(
            distanceInKm: 200m, distanceBasedPolicy.Id, zoneBasedPolicyZone2.Id,
            [firstClassModification.Id, seniorDiscountModification.Id]);

        await context.SoldProducts.AddRangeAsync(
            dailyPass1, dailyPass2, dailyPass3, dailyPass4,
            hybridTrip1, hybridTrip2, hybridTrip3, hybridTrip4);

        await context.SaveChangesAsync();
        
        logger.LogInformation("Database seeded successfully.");
    }
}