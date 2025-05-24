using System.ComponentModel.DataAnnotations;

namespace BarbershopBookApi.Domain;

public class CustomerModel
{
    [Key]
    public Guid Id { get; set; }
    [Phone]
    public string Phone { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}