using FareEngine.Domain.FarePolicies;
using FareEngine.Domain.Modifications;
using FareEngine.Domain.SoldProducts;
using Microsoft.EntityFrameworkCore;

namespace FareEngine.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<FarePolicy> FarePolicies { get; set; }
    public DbSet<SoldProduct> SoldProducts { get; set; }
    public DbSet<Modification> Modifications { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }    
}