using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Outbracket.Api.Contracts.Responses;
using Outbracket.Services.Contracts.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Outbracket.Globalization;

namespace WebApi.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json; charset=utf-8";
            response.StatusCode = (int)HttpStatusCode.OK;
            switch (exception)
            {
                case BusinessException businessException:
                    await response.WriteAsync(JsonSerializer.Serialize(new Response
                        {Code = ApiResponseCode.Error, ErrorMessage = businessException.Message}, new JsonSerializerOptions(){PropertyNamingPolicy = JsonNamingPolicy.CamelCase}));
                    break;
                case ValidationException validationException:
                    await response.WriteAsync(JsonSerializer.Serialize(new Response
                        { Code = ApiResponseCode.ValidationFailed, ValidationResult = validationException.ValidationErrors.Select(ToValidationResult).ToArray() }, new JsonSerializerOptions(){PropertyNamingPolicy = JsonNamingPolicy.CamelCase}));
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await response.WriteAsync(JsonSerializer.Serialize(new Response
                        { Code = ApiResponseCode.Error, ErrorMessage = Messages.UnhandledException.Item2 }, new JsonSerializerOptions(){PropertyNamingPolicy = JsonNamingPolicy.CamelCase}));
                    break;
            }
        }

        private static ValidationResult ToValidationResult(Tuple<string, string> tuple)
        {
            var (item1, item2) = tuple;
            return new ValidationResult(item1, item2);
        }
    }
}
