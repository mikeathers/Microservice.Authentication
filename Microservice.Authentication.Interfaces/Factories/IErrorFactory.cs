using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microservice.Authentication.Data.Configurations;

namespace Microservice.Authentication.Interfaces.Factories
{
    public interface IErrorFactory
    {
        Task LogError(string errorMessage, string functionName, ApplicationDbContext context);
    }
}
