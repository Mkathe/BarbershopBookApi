using BarbershopBookApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML.Messaging;
using Twilio.Types;

namespace BarbershopBookApi.Application.Services;

public class BookingService(IHairdresserRepository _repository, IConfiguration _configuration, ILogger<BookingService> _logger) : IBookingService
{
    public async Task<bool> BookHairdresserAsync(Guid hairdresserId, DateTime date, string customerPhone, string email)
    {
        var hairdresser = _repository.ToBook(hairdresserId, date).Result;
        if (hairdresser is null)
            return false;
        await SendSmsToHairdresser(hairdresser.Phone, customerPhone, date);
        return true;
    }

    public async Task SendSmsToHairdresser(string hairdresserPhone, string customerPhone, DateTime date)
    {
        var accountSid = _configuration["Twilio:TwilioSID"];
        var authToken = _configuration["Twilio:TwilioAuthToken"];
        TwilioClient.Init(accountSid,authToken);
        _logger.LogInformation(_configuration["Twilio:TwilioSID"]);
        _logger.LogInformation(_configuration["Twilio:TwilioAuthToken"]);
        _logger.LogInformation(_configuration["Twilio:TwilioPhone"]);
        _logger.LogInformation(hairdresserPhone);
        var message = await MessageResource.CreateAsync(
            from: new PhoneNumber(_configuration["Twilio:TwilioPhone"]),
            to: new PhoneNumber("+37068835525"), 
            body: $"💈 Новый клиент! Номер: {customerPhone}. Дата визита: {date:dd.MM.yyyy HH:mm}"
            );
    }
}