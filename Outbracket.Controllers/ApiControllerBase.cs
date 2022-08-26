using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Outbracket.Api.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Outbracket.Common.Extensions;
using Outbracket.Globalization;
using Outbracket.Services.Contracts.Exceptions;

namespace Outbracket.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected static Response Success()
        {
            return new Response
            {
                Code = ApiResponseCode.Success,
            };
        }
        protected static Response<T> Success<T>(T result)
        {
            return new Response<T>
            {
                Code = ApiResponseCode.Success,
                Result = result,
            };
        }

        protected AuthorizedUser GetUser()
        {
            var userId = User.FindFirst(x => x.Type == ClaimTypes.Name);
            if (userId == null)
            {
                throw new BusinessException(Messages.UserNotFound.Item2);
            }
            return new AuthorizedUser
            {
                Id = new Guid(userId.Value), 
                Roles = User.FindAll(x => x.Type == ClaimTypes.Role).ToEnumerable(x => x.Value)
            };
        }
    }
}
