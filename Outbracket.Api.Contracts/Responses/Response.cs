using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Api.Contracts.Responses
{
    public class Response
    {
        public ApiResponseCode Code { get; set; }

        public string ErrorMessage { get; set; }

        public ValidationResult[] ValidationResult { get; set; }
    }

    public class Response<T> : Response
    {
        public T Result { get; set; }
    }
}
