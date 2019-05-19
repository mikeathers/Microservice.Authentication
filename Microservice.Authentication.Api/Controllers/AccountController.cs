using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Authentication.Api.Helpers.Interfaces;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Account;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto, [FromServices] IRegisterAccountService service)
        {
            var registerValidator = new EmailValidator();
            var validatorResult = await registerValidator.ValidateAsync(registerDto);

            if (validatorResult.IsValid)
            {
                await service.RegisterAccount(registerDto);
                if (!service.Status.HasErrors)
                {
                    return Ok();
                }
                return BadRequest(service.Status.Errors);
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();
            
            return BadRequest(validationErrors);
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, [FromServices] ILoginService service)
        {
            var loginValidator = new LoginValidator();
            var validatorResult = await loginValidator.ValidateAsync(loginDto);

            if (validatorResult.IsValid)
            {
                var registeredUser = await service.Login(loginDto);
                if (!service.Status.HasErrors)
                {
                    return Ok(registeredUser);
                }

                return BadRequest(service.Status.Errors);
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            return BadRequest(validationErrors);

        }

        [HttpGet, Route("confirmemail")]
        public async Task<IActionResult> ConfirmEmail([FromQuery(Name = "id")] string userId, [FromQuery(Name = "code")] string emailConfirmationToken, [FromServices] IConfirmEmailService service, [FromServices] ICreateDtoService createDtoService)
        {
            var confirmEmailDto = createDtoService.ConfirmEmailDto(userId, emailConfirmationToken);

            var confirmEmailValidator = new ConfirmEmailValidator();
            var validatorResult = await confirmEmailValidator.ValidateAsync(confirmEmailDto);

            if (validatorResult.IsValid)
            {
                await service.ConfirmEmail(confirmEmailDto);
                if (!service.Status.HasErrors)
                {
                    return Ok();
                }
                return BadRequest(service.Status.Errors);
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            return BadRequest(validationErrors);

        }

        [HttpGet, Route("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto, [FromServices] IForgotPasswordService service)
        {
            var forgotPasswordValidator = new ForgotPasswordValidator();
            var validatorResult = await forgotPasswordValidator.ValidateAsync(forgotPasswordDto);

            if (validatorResult.IsValid)
            {
                await service.RequestPasswordReset(forgotPasswordDto);
                if (!service.Status.HasErrors)
                {
                    return Ok();
                }
                return BadRequest(service.Status.Errors);
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            return BadRequest(validationErrors);

        }

        [HttpGet, Route("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromServices] IResetPasswordService service)
        {
            var resetPasswordValidator = new ResetPasswordValidator();
            var validatorResult = await resetPasswordValidator.ValidateAsync(resetPasswordDto);

            if (validatorResult.IsValid)
            {
                await service.ResetPassword(resetPasswordDto);
                if (!service.Status.HasErrors)
                {
                    return Ok();
                }
                return BadRequest(service.Status.Errors);
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            return BadRequest(validationErrors);

        }
    }
}