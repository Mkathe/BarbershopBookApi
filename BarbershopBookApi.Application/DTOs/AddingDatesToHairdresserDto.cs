using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BarbershopBookApi.Domain.Converter;

namespace BarbershopBookApi.Application.DTOs;

public class AddingDatesToHairdresserDto
{
    [JsonConverter(typeof(CustomDateTimeConverter))]
    [Required(ErrorMessage = "Free date time is required!")]
    public List<DateTime> FreeDateTime { get; set; } = new();
    [Required(ErrorMessage = "The last name is required!")]
    public string LastName { get; set; } = string.Empty;
}