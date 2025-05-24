namespace BarbershopBookApi.Application.DTOs;

public class RefreshTokenRequestDto
{
    public Guid AdminId { get; set; }
    public required string RefreshToken { get; set; }
}