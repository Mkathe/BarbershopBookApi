using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BarbershopBookApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HairdresserController(IHairdresserRepository _repository, IBookingService _bookingService) : ControllerBase
{
    [HttpGet("hairdressers")]
    [ProducesResponseType(200)]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<HairdresserModel>>> Get()
    {
        var result = await _repository.GetHairdressers();
        return Ok(result);
    }

    [HttpGet("hairdresser/{id:guid}", Name = nameof(GetHairdresserById))]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]

    public async Task<IActionResult> GetHairdresserById([FromRoute] Guid id)
    {
        var result = await _repository.GetHairdresserById(id);
        if (result is null)
            return NotFound("Hairdresser is not found");
        return Ok(result);
    }
    [HttpGet("hairdresser/lastName")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]

    public async Task<IActionResult> GetHairdresserByLastName(string lastName)
    {
        var result = await _repository.GetHairdresserByLastName(lastName);
        if (result is null)
            return NotFound("Hairdresser is not found");
        return Ok(result);
    }
    [HttpGet("hairdresser/check/isAvailable")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]

    public async Task<ActionResult<bool>> IsHairdresserFreeAtTheChoosingDate(string lastName, DateTime date)
    {
        var result = await _repository.IsHairdresserFreeAtTheChoosingDate(lastName, date);
        if (result is false)
            return NotFound("Hairdresser is not found or busy");
        return Ok(result);
    }
    [HttpPost]
    [Authorize]
    [ProducesResponseType(201)]
    public async Task<IActionResult> AddHairdresser([FromBody] HairdresserDto hairdresserDto)
    {
        var hairdresser = await _repository.AddHairdresser(hairdresserDto);
        return CreatedAtRoute(nameof(GetHairdresserById), new {id = hairdresser.Id}, hairdresser);
    }
    
    [HttpPut("hairdresser/update/{id:guid}")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> UpdateHairdresser([FromRoute] Guid id, [FromBody] HairdresserDto hairdresserDto)
    {
        var result = await _repository.UpdateHairdresser(id, hairdresserDto);
        if (result is null)
            return BadRequest("The hairdresser is not found or null");
        return NoContent();
    }
    [HttpPost("hairdressers/booking")]
    [AllowAnonymous]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> BookHairdresser([FromBody] BookingRequestDto bookingRequestDto)
    {
        var success = await _bookingService.BookHairdresserAsync(bookingRequestDto);
        if (!success)
            return BadRequest("Hairdresser is already booked or not found on that date");
        return Ok("Booking successful. Email is sending...");
    }
    [HttpPatch("hairdressers/booking/unbook/{lastname}")]
    [Authorize]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UnBookHairdresser([FromRoute] string lastname)
    {
        var model = await _repository.ToUnBook(lastname);
        if (model is null)
            return BadRequest("Hairdresser's lastname is misspelled or not found");
        return Ok(model);
    }
    [HttpDelete("hairdresser/delete/{id:guid}")]
    [Authorize]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteHairdresser([FromRoute] Guid id)
    {
        var result = await _repository.DeleteHairdresser(id);
        if (result is null)
            return NotFound("The hairdresser is not found");
        return NoContent();
    }
}