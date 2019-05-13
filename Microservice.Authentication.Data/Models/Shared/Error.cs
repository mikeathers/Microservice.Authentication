using System;

namespace Microservice.Authentication.Data.Models.Shared
{
    public class Error
    {
        public int ErrorId { get; set; }
        public string Message { get; set; }
        public string FunctionName { get; set; }
        public DateTime DateCreated { get; set; }

        public Error(string message, string functionName)
        {
            Message = message;
            FunctionName = functionName;
            DateCreated = DateTime.Now;
        }
    }
}
