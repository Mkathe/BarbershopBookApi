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
}