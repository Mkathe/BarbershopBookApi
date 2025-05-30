using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarbershopBookApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IAdminRepository _repository;
    private readonly IAuthService _authService;

    public AdminController(IAuthService authService, IAdminRepository repository)
    {
        _authService = authService;
        _repository = repository;
    }
    [HttpGet("admins")]
    [Authorize]
    public async Task<IActionResult> GetAdmins()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var admins = await _repository.GetAdmins();
        return Ok(admins);
    }

    [HttpGet("admins/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetAdminById([FromRoute] Guid Id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _repository.GetAdmin(id: Id);
        return Ok(result);
    }
    [HttpPost("register")]
    [Authorize]
    public async Task<ActionResult<AdminModel>> Register(AdminDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _authService.RegisterAdmin(request);
        if (user == null)
        {
            return BadRequest("Admin already exists");
        }
        return Ok(user);
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponseDto>> Login(AdminDto adminDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _authService.LoginAdmin(adminDto);
        if (result is null)
            return BadRequest("The username or password is invalid");
        return Ok(result);
    }
    [HttpPost("RefreshToken")]
    [Authorize]
    public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var result = await _authService.RefreshTokenAsync(request);
        if (result is null || result.AccessToken is null || result.RefreshToken is null)
        {
            return Unauthorized("Invalid refresh token");
        }
        return Ok(result);
    }
    [HttpGet("Check")]
    [Authorize]
    public IActionResult AuthenticatedOnlyEndpoint()
    {
        return Ok("You are authenticated");
    }
    [Authorize(Roles = "Admin")]
    [HttpGet("AdminOnly")]
    public IActionResult AdminOnlyEndpoint()
    {
        return Ok("You are admin!");
    }
}