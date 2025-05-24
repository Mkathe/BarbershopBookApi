using BarbershopBookApi.Domain;

namespace BarbershopBookApi.Application.Interfaces;

public interface ICustomerRepository
{
    Task<IEnumerable<CustomerModel>> GetCustomers();
    Task<CustomerModel> GetCustomer(Guid id);
    Task<CustomerModel> AddCustomer(CustomerModel customer);
}