using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BarbershopBookApi.Domain.Converter;

namespace BarbershopBookApi.Application.DTOs;

public class BookingRequestDto
{
    [Required(ErrorMessage = "The last name is required!")] 
    public string HairdresserLastName { get; set; } = string.Empty;
    [Required(ErrorMessage = "The date is required!")]
    public DateTime ChoosingDateTime { get; set; }
    [Phone(ErrorMessage = "Mobile no. is invalid!")]
    [Required(ErrorMessage = "Mobile no. is required!")]
    [Length(11,12,ErrorMessage = "Mobile no. must be 11 or 12 digits!")]
    public string CustomerPhone { get; set; } = string.Empty;
    [EmailAddress(ErrorMessage = "Email is invalid!")]
    [Required(ErrorMessage = "Email is required!")]
    public string CustomerEmail { get; set; } = string.Empty;

}