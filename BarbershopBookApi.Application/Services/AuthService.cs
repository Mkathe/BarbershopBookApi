using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using IAuthorizationService = Microsoft.AspNetCore.Authorization.IAuthorizationService;

namespace BarbershopBookApi.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IAdminRepository _adminRepository;
    private readonly ILogger<AuthService> _logger;
    public AuthService(IConfiguration configuration, ILogger<AuthService> logger, IAdminRepository adminRepository)
    {
        _configuration = configuration;
        _logger = logger;
        _adminRepository = adminRepository;
    }

    public async Task<AdminModel?> RegisterAdmin(AdminDto adminDto)
    {
        if (await _adminRepository.IsAdminExist(adminDto))
            return null;
        var adminModel = new AdminModel();
        var hasedPassword = new PasswordHasher<AdminModel>()
            .HashPassword(adminModel, adminDto.Password);
        adminModel.UserName = adminDto.UserName;
        adminModel.PasswordHashed = hasedPassword;
        await _adminRepository.AddAdmin(adminModel);
        return adminModel;
    }

    public async Task<TokenResponseDto?> LoginAdmin(AdminDto adminDto)
    {
        var admin = _adminRepository.FindAdminByName(adminDto).Result;
        if (admin is null)
            return null;
        if (admin.UserName != adminDto.UserName)
            return null;
        if (new PasswordHasher<AdminModel>().VerifyHashedPassword(admin, admin.PasswordHashed, adminDto.Password) ==
            PasswordVerificationResult.Failed)
        {
            return null;
        }
        return await CreateTokenResponse(admin);
    }

    private async Task<TokenResponseDto?> CreateTokenResponse(AdminModel admin)
    {
        var response = new TokenResponseDto()
        {
            AccessToken = CreateToken(admin),
            RefreshToken =  await _adminRepository.GenerateAndSaveRefreshToken(admin)
        };
        return response;
    }
    /*private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }*/

    private string CreateToken(AdminModel admin)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, admin.UserName),
            new Claim(ClaimTypes.Role, admin.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Token"] ?? string.Empty));
        if (key is null)
            _logger.LogError("Key is null");
        var creds = new SigningCredentials(key, algorithm: SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new JwtSecurityToken(
            claims: claims,
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            signingCredentials: creds,
            expires: DateTime.UtcNow.AddDays(3)
            );
        if (tokenDescriptor.Audiences is null || tokenDescriptor.Issuer is null)
        {
            _logger.LogError("TokenDescriptor is null");
            return string.Empty;
        }
        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return token;
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var admin = await ValidateRefreshToken(request.AdminId, request.RefreshToken);
        if (admin is null)
            return null;
        return await CreateTokenResponse(admin);
    }
    private async Task<AdminModel?> ValidateRefreshToken(Guid Id, string refreshToken)
    {
        var admin = await _adminRepository.FindAdminById(Id);
        if (admin is null || admin.RefreshToken != refreshToken || admin.RefreshTokenExpiryTime <= DateTime.Now)
            return null!;
        return admin;
    }
}