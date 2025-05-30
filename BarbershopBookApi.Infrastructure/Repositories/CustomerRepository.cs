using BarbershopBookApi.Application.DTOs;
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
    public async Task<CustomerModel?> GetCustomerById(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null)
            return null!;
        return customer;
    }
    public async Task<CustomerModel?> GetCustomerByEmail(string email)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);
        if (customer is null)
            return null!;
        return customer;
    }
    public async Task<CustomerModel> AddCustomer(CustomerDto customerDto)
    {
        var customer = new CustomerModel()
        {   
            Id = Guid.NewGuid(),
            Email = customerDto.Email,
            Phone = customerDto.Phone
        };
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }
    public async Task AddCustomerAfterBooking(BookingRequestDto bookingRequestDto)
    {
        var customer = new CustomerModel()
        {   
            Id = Guid.NewGuid(),
            Email = bookingRequestDto.CustomerEmail,
            Phone = bookingRequestDto.CustomerPhone,
            ChoosedDate = bookingRequestDto.ChoosingDateTime
        };
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<CustomerModel?> DeleteCustomer(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer is null)
            return null!;
        _context.Customers.Remove(customer);
        _context.SaveChangesAsync();
        return customer;
    }
}