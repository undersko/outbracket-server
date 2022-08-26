using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Services.Contracts.Exceptions
{
    public class ValidationException : Exception
    {
        public Tuple<string, string>[] ValidationErrors { get; set; }

        public ValidationException(Tuple<string, string>[] errors)
        {
            ValidationErrors = errors;
        }
    }
}
