using System;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.Config;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Microservice.Authentication.Services.Shared
{
    public class SendGridClientService : ISendGridClientService
    {
        private readonly SendGridSettings _sendGridSettings;
        public StatusGenericHandler Status { get; }
        
        public SendGridClientService(IOptions<SendGridSettings> options)
        {
            Status = new StatusGenericHandler();
            _sendGridSettings = options.Value;
        }

        public SendGridClient Create()
        {
            var key = _sendGridSettings.Key;
            var client = new SendGridClient(key);
            return client;
        }

        public SendGridMessage CreateSingleEmail(EmailAddress fromAddress, EmailAddress toAddress, string subject, string plainTextContent, string htmlContent)
        {
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, plainTextContent, htmlContent);
            return message;
        }

        public async Task<Response> SendEmailAsync(SendGridMessage message, SendGridClient client)
        {
            var response = await client.SendEmailAsync(message);
            return response;
        }

        public EmailAddress CreateFromAddress()
        {
            return new EmailAddress(_sendGridSettings.FromEmail, _sendGridSettings.FromName);
        }

        public EmailAddress CreateToAddress(string toAddress, string toName)
        {
            return new EmailAddress(toAddress, toName);
        }
    }
}