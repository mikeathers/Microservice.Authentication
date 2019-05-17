using System;
using System.Net;
using System.Threading.Tasks;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;

namespace Microservice.Authentication.Services.Shared
{
    public class SendEmailService : ISendEmailService
    {
        private readonly ISendGridClientService _sendGridClientService;
        public StatusGenericHandler Status { get; }

        public SendEmailService(ISendGridClientService sendGridClientService)
        {
            Status = new StatusGenericHandler();
            _sendGridClientService = sendGridClientService;
        }

        public async Task SendAsync(string email, string subject, string body, string toName = null)
        {
            try
            {
                if (string.IsNullOrEmpty(email)) Status.AddError("An email address to send the message to has not been provided.");
                if (string.IsNullOrEmpty(subject)) Status.AddError("A subject for the email has not been provided.");
                if (string.IsNullOrEmpty(body)) Status.AddError("A body for the email has not been provided.");
                if (Status.HasErrors) return;

                var client = _sendGridClientService.Create();
                var from = _sendGridClientService.CreateFromAddress();
                var to =_sendGridClientService.CreateToAddress(email, toName);

                var message = _sendGridClientService.CreateSingleEmail(from, to, subject, body, body);
                var response = await _sendGridClientService.SendEmailAsync(message, client);

                if (response.StatusCode == HttpStatusCode.Accepted) return;

                Status.AddError(
                    $"An error occured whilst trying to send an email using SendGrid. Status: {response.StatusCode}");

            }
            catch (Exception e)
            {
                Status.AddError(e.Message);
                return;
            }
        }
    }
}
