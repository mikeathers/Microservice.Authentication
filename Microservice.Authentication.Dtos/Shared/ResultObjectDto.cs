using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;

namespace Microservice.Authentication.Dtos.Shared
{
    public class ResultObjectDto
    {
        public IImmutableList<ValidationResult> Errors { get; set; }
        public bool HasErrors { get; set; }
        public object Result { get; set; }

        public ResultObjectDto(bool hasErrors, object obj = null, IImmutableList<ValidationResult> errors = null)
        {
            Errors = errors;
            HasErrors = hasErrors;
            Result = obj;
        }
    }
}
