using BarbershopBookApi.Application.DTOs;

namespace BarbershopBookApi.Application.Interfaces;

public interface IBookingService
{
    Task<bool> BookHairdresserAsync(BookingRequestDto request);
    /*Task SendSmsToHairdresser(string hairdresserPhone, string customerPhone, DateTime date);*/
    Task SendEmailAsync(string senderEmail, string senderName, string receiverEmail,
        string receiverName, string subject, string message);
}