using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace BarbershopBookApi.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<CustomerModel>> GetCustomers()
    {
        if (await _context.Customers.AnyAsync(x => x.Id == Guid.Empty))
            return null!;
        var customers = await _context.Customers.ToListAsync();
        return customers;
    }
    public async Task<CustomerModel> GetCustomer(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null)
            return null!;
        return customer;
    }
    public async Task<CustomerModel> AddCustomer(CustomerModel customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
}