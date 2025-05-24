namespace BarbershopBookApi.Application.Interfaces;

public interface IBookingService
{
    Task<bool> BookHairdresserAsync(Guid hairdresserId, DateTime date, string customerPhone, string email);
    Task SendSmsToHairdresser(string hairdresserPhone, string customerPhone, DateTime date);
}