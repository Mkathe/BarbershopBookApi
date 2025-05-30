using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BarbershopBookApi.Domain.Converter;

namespace BarbershopBookApi.Domain;

public class CustomerModel
{
    [Key]
    public Guid Id { get; set; }
    [Phone]
    public string Phone { get; set; } = string.Empty;
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    public DateTime ChoosedDate { get; set; }
}