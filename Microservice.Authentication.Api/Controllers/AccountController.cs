using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microservice.Authentication.Api.Validators.Account;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Dtos.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Authentication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpPost, Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerValidator = new RegisterValidator();
            var validatorResult = registerValidator.Validate(registerDto);

            if (validatorResult.IsValid)
            {

            }

            var validationErrors = validatorResult.Errors.Select(error => new ValidationResult(error.ErrorMessage))
                .ToImmutableList();

            var badRequestObj = new ResultObjectDto(true, null, validationErrors);
            return new BadRequestObjectResult(badRequestObj);
        }
    }
}