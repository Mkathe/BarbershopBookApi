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
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCustomers()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _repository.GetCustomers();
        return Ok(result);
    }

    [HttpGet("customer/{id:guid}", Name = nameof(GetCustomerById))]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _repository.GetCustomerById(id);
        if (result is null)
            return NotFound("No customer is found");
        return Ok(result);
    }
    [HttpGet("customer/{email}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCustomerByEmail(string email)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _repository.GetCustomerByEmail(email);
        if (result is null)
            return NotFound("No customer is found");
        return Ok(result);
    }
    [HttpPost]
    [Authorize]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> AddCustomer([FromBody] CustomerDto customerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var customer = await _repository.AddCustomer(customerDto);
        return CreatedAtRoute(nameof(GetCustomerById), new { id = customer.Id}, customer);
    }

    [HttpDelete("delete/{id:guid}")] 
    [Authorize] 
    [ProducesResponseType(204)] 
    [ProducesResponseType(400)]
    public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var customer = await _repository.DeleteCustomer(id);
        if (customer is null)
            return NotFound("No customer is found");
        return NoContent();
    }
}