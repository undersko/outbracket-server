#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.Results;
using Outbracket.Common.Extensions;
using Outbracket.Globalization;
using Outbracket.Globalization.Helpers;

namespace Outbracket.Controllers.Validators.Common
{
    public abstract class AbstractValidatorCustom<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var validationResult = base.Validate(context);

            if (!validationResult.IsValid)
            {
                throw new Outbracket.Services.Contracts.Exceptions.ValidationException(validationResult.Errors.ToNotNullArray(ToValidationResult));
            }

            return validationResult;
        }
        
        private static Tuple<string, string>? ToValidationResult(ValidationFailure failure)
        {
            var validationError = typeof(ValidationErrors).GetGlobalizationField(failure.ErrorCode);
            object?[] validationErrorParams =
                ((failure.CustomState as IEnumerable<string>) ?? Array.Empty<string>()).ToArray();
            return validationError == null ? 
                null : 
                new Tuple<string, string>(validationError.Item1, string.Format(validationError.Item2, validationErrorParams));
        }
    }
}