using System;
using System.Collections.Generic;
using System.Text;

namespace Outbracket.Services.Contracts.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base(message)
        {
            
        }
    }
}
