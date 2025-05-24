using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarbershopBookApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerRepository _repository;

    public CustomerController(ICustomerRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("customers")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await _repository.GetCustomers();
        return Ok(result);
    }

    [HttpGet("customer/{id:guid}", Name = nameof(GetCustomerById))]
    [AllowAnonymous]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var result = await _repository.GetCustomer(id);
        if (result is null)
            return NotFound("No customer is found");
        return Ok(result);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
    {
        var customer = await _repository.AddCustomer(customerDto);
        return CreatedAtRoute(nameof(GetCustomerById), new { id = customer.Id}, customer);
    }
}