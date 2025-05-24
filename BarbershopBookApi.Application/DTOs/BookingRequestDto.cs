using System.ComponentModel.DataAnnotations;

namespace BarbershopBookApi.Application.DTOs;

public class BookingRequestDto
{
    public Guid HairdresserId { get; set; }
    public DateTime Date { get; set; }
    [Phone]
    public string CustomerPhone { get; set; } = string.Empty;
    [EmailAddress]
    public string CustomerEmail { get; set; } = string.Empty;

}