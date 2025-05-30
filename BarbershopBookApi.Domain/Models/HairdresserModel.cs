using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BarbershopBookApi.Domain.Converter;

namespace BarbershopBookApi.Domain;

public class HairdresserModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string HiredIn { get; set; } = string.Empty;
    [Phone]
    public string Phone { get; set; } = string.Empty;
    [EmailAddress] 
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public List<DateTime> FreeDateTime { get; set; } = new();
    public bool IsBooked { get; set; } = false;
}