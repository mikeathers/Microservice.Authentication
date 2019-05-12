using System;

namespace Microservice.Authentication.Data.Models.Shared
{
    public class Error
    {
        public int ErrorId { get; set; }
        public string Message { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
