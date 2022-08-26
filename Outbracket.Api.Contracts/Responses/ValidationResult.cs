using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Responses
{
    public class ValidationResult
    {
        public ValidationResult(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }

        public string Message { get; set; }
    }
}
