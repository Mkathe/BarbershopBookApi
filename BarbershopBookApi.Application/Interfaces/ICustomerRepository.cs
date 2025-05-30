using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Domain;

namespace BarbershopBookApi.Application.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerModel>> GetCustomers();
    Task<CustomerModel?> GetCustomerById(Guid id);
    Task<CustomerModel?> GetCustomerByEmail(string email);
    Task<CustomerModel> AddCustomer(CustomerDto Dto);
    Task AddCustomerAfterBooking(BookingRequestDto bookingRequestDto);
    Task<CustomerModel?> DeleteCustomer(Guid id);
}