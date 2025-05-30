using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarbershopBookApi.Infrastructure;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<CustomerModel> Customers => Set<CustomerModel>();
    public DbSet<AdminModel> Admins => Set<AdminModel>();
    public DbSet<HairdresserModel> Hairdressers => Set<HairdresserModel>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HairdresserModel>()
            .Property(x => x.FreeDateTime)
            .HasColumnType("timestamp[]");
        modelBuilder.Entity<CustomerModel>()
            .Property(x => x.ChoosedDate)
            .HasColumnType("timestamp");
    }
}