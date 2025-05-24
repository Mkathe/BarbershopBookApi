using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Domain;

namespace BarbershopBookApi.Application.Interfaces;

public interface IAuthService
{
    Task<AdminModel?> RegisterAdmin(AdminDto adminDto);
    Task<TokenResponseDto?> LoginAdmin(AdminDto admin);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
}