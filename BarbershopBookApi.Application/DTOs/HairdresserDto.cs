using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BarbershopBookApi.Domain.Converter;

namespace BarbershopBookApi.Application.DTOs;

public class HairdresserDto
{
    [Required(ErrorMessage = "The first name is required!")]
    public string FirstName { get; set; } = string.Empty;
    [Required(ErrorMessage = "The last name is required!")]
    public string LastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Hired in is required!")]
    public string HiredIn { get; set; } = string.Empty;
    [Phone(ErrorMessage = "Mobile no. is invalid!")]
    [Required(ErrorMessage = "Phone is required!")]
    [Length(11,12,ErrorMessage = "Mobile no. must be 11 or 12 digits!")]
    public string Phone { get; set; } = string.Empty;
    [EmailAddress(ErrorMessage = "Email is invalid!")]
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Address is required!")]
    public string Address { get; set; } = string.Empty;

    [JsonConverter(typeof(CustomDateTimeConverter))]
    [Required(ErrorMessage = "Free date time is required!")]
    public List<DateTime> FreeDateTime { get; set; } = new();
    [Required(ErrorMessage = "The field of is booked is required!")]
    public bool IsBooked { get; set; }
}