using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Dtos.Shared;
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
            var registerValidator = new RegisterValidator();
            var validatorResult = registerValidator.Validate(registerDto);

            if (validatorResult.IsValid)
            {
                await service.RegisterAccount(registerDto);
                if (!service.Status.HasErrors)
                {
                    return new OkObjectResult(new ResultObjectDto(false, null));
                }
                return new BadRequestObjectResult(new ResultObjectDto(true, null, service.Status.Errors));
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            var badRequestObj = new ResultObjectDto(true, null, validationErrors);
            return new BadRequestObjectResult(badRequestObj);
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, [FromServices] ILoginService service)
        {
            var loginValidator = new LoginValidator();
            var validatorResult = loginValidator.Validate(loginDto);

            if (validatorResult.IsValid)
            {
                var registeredUser = await service.Login(loginDto);
                if (!service.Status.HasErrors)
                {
                    return new OkObjectResult(new ResultObjectDto(false, registeredUser));
                }
                return new BadRequestObjectResult(new ResultObjectDto(true, registeredUser, service.Status.Errors));
            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            var badRequestObj = new ResultObjectDto(true, null, validationErrors);
            return new BadRequestObjectResult(badRequestObj);

        }
    }
}