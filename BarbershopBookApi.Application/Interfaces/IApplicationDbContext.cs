using BarbershopBookApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarbershopBookApi.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CustomerModel> Customers { get;  }
    DbSet<AdminModel> Admins { get;  }
    DbSet<HairdresserModel> Hairdressers { get;  }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}