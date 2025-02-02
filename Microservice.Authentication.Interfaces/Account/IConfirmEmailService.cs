﻿using System.Threading.Tasks;
using Microservice.Authentication.Dtos.Account;
using Microservice.Authentication.Interfaces.Generic;

namespace Microservice.Authentication.Interfaces.Account
{
    public interface IConfirmEmailService
    {
        StatusGenericHandler Status { get; }
        Task ConfirmEmail(ConfirmEmailDto confirmEmailDto);
    }
}