using System.ComponentModel.DataAnnotations;

namespace BarbershopBookApi.Domain;

public class AdminModel
{
    [Key]
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordHashed { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime? RefreshTokenExpiryTime { get; set; }
}