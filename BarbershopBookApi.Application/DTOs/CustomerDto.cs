using System.ComponentModel.DataAnnotations;

namespace BarbershopBookApi.Application.DTOs;

public class CustomerDto
{
    [Phone(ErrorMessage = "Mobile no. is invalid!")]
    [Required(ErrorMessage = "Mobile no. is required!")]
    [Length(11,12,ErrorMessage = "Mobile no. must be 11 or 12 digits!")]
    public string Phone { get; set; } = string.Empty;
    [EmailAddress(ErrorMessage = "Email is invalid!")]
    [Required(ErrorMessage = "Email is required!")]
    public string Email { get; set; } = string.Empty;
}