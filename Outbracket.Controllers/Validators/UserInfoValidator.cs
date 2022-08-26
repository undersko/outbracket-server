using System;
using System.Collections.Generic;
using FluentValidation;
using Outbracket.Api.Contracts.Requests.Account;
using Outbracket.Controllers.Validators.Common;

namespace Outbracket.Controllers.Validators
{
    public class UserInfoValidator : AbstractValidatorCustom<UpdateUserInfoApiRequest>
    {
        public UserInfoValidator()
        {
            RuleFor(userInfo => userInfo.Bio).MaximumLength(50).WithErrorCode("MaximumLength").WithState(person => new List<string> {"50"});
        }
    }
}