using BarbershopBookApi.Application.DTOs;
using BarbershopBookApi.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

namespace BarbershopBookApi.Application.Services;

public class BookingService(IHairdresserRepository _repository, ICustomerRepository _customerRepository, IConfiguration _configuration, ILogger<BookingService> _logger) : IBookingService
{
    public async Task<bool> BookHairdresserAsync(BookingRequestDto request)
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken token = cts.Token;
        var hairdresser = _repository.ToBook(request.HairdresserLastName, request.ChoosingDateTime).Result;
        if (hairdresser is null)
            return false;
        await _customerRepository.AddCustomerAfterBooking(request).WaitAsync(token);
        var customer = await _customerRepository.GetCustomerByEmail(request.CustomerEmail);
        var hairdresserEmail = hairdresser.Email;
        _logger.LogWarning("API: {key}\nSenderEmail: {email}\n SenderName: {senderName}",_configuration["BrevoApi:ApiKey"], _configuration["BrevoApi:SenderEmail"], _configuration["BrevoApi:SenderName"]);
        var messageForHairdresser = $"Here is customer email: {request.CustomerEmail}\nThe customer phone: {request.CustomerPhone}\nThe date is {request.ChoosingDateTime:dd.MM.yyyy HH:mm}";
        var messageForCustomer = $"Welcome to our barbershop website!\nThank you for using our booking service!\nHere are details of booking:\nMaster's name: {hairdresser.FirstName}\nMaster's phone: {hairdresser.Phone}\nAddress: {hairdresser.Address} - {hairdresser.HiredIn}\nDate: {customer.ChoosedDate:dd.MM.yyyy HH:mm}\n";
        var subject = "New booking to hairdresser";
        await SendEmailAsync(_configuration["BrevoApi:SenderEmail"] ?? string.Empty,
            _configuration["BrevoApi:SenderName"] ?? string.Empty, 
            hairdresserEmail,
            request.CustomerPhone,
            subject,
            messageForHairdresser);
        await SendEmailAsync(_configuration["BrevoApi:SenderEmail"] ?? string.Empty,
            _configuration["BrevoApi:SenderName"] ?? string.Empty, 
            customer.Email,
            hairdresser.Phone,
            subject,
            messageForCustomer);
        return true;
    }
    public async Task SendEmailAsync(string senderEmail, string senderName, string receiverEmail,
        string receiverName, string subject, string message)
    {
        var apiInstance = new TransactionalEmailsApi();
        SendSmtpEmailSender sender = new SendSmtpEmailSender(senderName, senderEmail);
        SendSmtpEmailTo receiver1 = new SendSmtpEmailTo(receiverEmail, receiverName);
        List<SendSmtpEmailTo> To = new List<SendSmtpEmailTo>();
        To.Add(receiver1);
        string HtmlContent = null;
        string TextContent = message;
        try
        {
            var sendSmtpEmail = new SendSmtpEmail(sender, To, null, null, HtmlContent, TextContent, subject);
            CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
            _logger.LogInformation(result.ToJson());
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}