using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Models.Config;
using Microservice.Authentication.Dtos.Shared;
using Microservice.Authentication.Interfaces.Generic;
using Microservice.Authentication.Interfaces.Shared;
using Microservice.Authentication.Services.Shared;
using Microsoft.Extensions.Options;
using Moq;
using SendGrid;
using SendGrid.Helpers.Mail;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Microservice.Authentication.Tests.UnitTests.Services.SharedTests
{
    public class SendEmailServiceTests
    {
        public static IEnumerable<object[]> EmailData => new List<object[]>
        {
            new []{"test.user@test.com", "test subject", ""},
            new []{"test.user@test.com", "", "test body"},
            new []{"", "test subject", "test body"}
        };
        
        [Fact]
        public async Task Should_Use_SendGrid_ToSendEmail()
        {
            // Arrange
            var sendEmailInfo = new SendEmailDto
            {
                Email = "test@test.com",
                Body = "Hello there",
                Subject = "Test Email",
                FromName = "My Company",
                ToName = "Test User"
            };

            var options = Options.Create(new SendGridSettings()
            {
                Key= "testKey",
                FromEmail = "athers_05@hotmail.co.uk",
                FromName = "Mike"

            });

            var client = new SendGridClient(options.Value.Key);
            var message = new SendGridMessage();
            var from = new EmailAddress(options.Value.FromEmail, options.Value.FromName);
            var to = new EmailAddress(sendEmailInfo.Email, sendEmailInfo.ToName);

            var mockHttpContent = new Mock<HttpContent>();
            var response = new Response(HttpStatusCode.OK, mockHttpContent.Object, null);

            var mockSendGridService = new Mock<ISendGridClientService>();

            mockSendGridService.Setup(m => m.CreateToAddress(sendEmailInfo.Email, sendEmailInfo.ToName))
                .Returns(to);

            mockSendGridService.Setup(m => m.CreateFromAddress())
                .Returns(from);

            mockSendGridService.Setup(m => m.Create()).Returns(client);

            mockSendGridService.Setup(m =>
                m.CreateSingleEmail(from, to, sendEmailInfo.Subject, sendEmailInfo.Body, sendEmailInfo.Body)).Returns(message);

            mockSendGridService.Setup(m => m.SendEmailAsync(message, client)).Returns(Task.FromResult(response));

            var sut = new SendEmailService(mockSendGridService.Object);

            // Act
            await sut.SendAsync(sendEmailInfo.Email, sendEmailInfo.Subject, sendEmailInfo.Body, sendEmailInfo.ToName);

            // Assert
            mockSendGridService.Verify(m => m.SendEmailAsync(message, client), Times.Once);
        }
        
        [Theory, MemberData(nameof(EmailData))]
        public async Task Should_HaveErrors_WhenRequiredInfo_NotProvided(string email, string subject, string body)
        {
            // Arrange
            var mockSendGridService = new Mock<ISendGridClientService>();
            mockSendGridService.Setup(m => m.Status).Returns(new StatusGenericHandler());

            var sut = new SendEmailService(mockSendGridService.Object);

            // Act
            await sut.SendAsync(email, subject, body);

            // Assert
            sut.Status.HasErrors.ShouldBeTrue();
        }
    }
}
