using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BarbershopBookApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class HairdresserController(IHairdresserRepository _repository) : ControllerBase
{
    [HttpGet("hairdressers")]
    public async Task<ActionResult<IEnumerable<HairdresserModel>>> Get()
    {
        var result = await _repository.GetHairdressers();
        if (result is null)
            return BadRequest("No hairdressers found");
        return Ok(result);
    }
}