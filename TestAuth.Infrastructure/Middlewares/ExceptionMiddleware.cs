using AutoMapper;

using FluentValidation;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

using TestAuth.Domain.Model.Exceptions;

namespace Tion.Map.AspNetCore.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public static readonly Dictionary<string, string> ProblemDetailTypes = new Dictionary<string, string>
        {
            { "3.1", "https://tools.ietf.org/html/rfc7235#section-3.1" },
            { "6.5.4", "https://tools.ietf.org/html/rfc7231#section-6.5.4" },
            { "6.6.1", "https://tools.ietf.org/html/rfc7231#section-6.6.1" }
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await TranslateExceptionAsync(ex, httpContext);
            }
        }

        protected virtual async Task TranslateExceptionAsync(Exception exception, HttpContext httpContext)
        {
            if (exception is AutoMapperMappingException automapperExc)
            {
                await TranslateExceptionAsync(automapperExc.InnerException ?? automapperExc, httpContext);
            }
            else if (exception is ValidationException valEx)
            {
                var title = "Validation exception.";
                var appEx = new AppException(title, exception.Message, valEx.GetErrorsDictionary(), valEx);

                _logger.LogError(appEx, title);
                await WriteResponse(httpContext, appEx, ProblemDetailTypes["6.6.1"], StatusCodes.Status400BadRequest);
            }
            else if (exception is UnauthorizedAppException uaEx)
            {
                _logger.LogError(uaEx, $"Unauthorized exception.");
                await WriteResponse(httpContext, uaEx, ProblemDetailTypes["3.1"], StatusCodes.Status401Unauthorized);
            }
            else if (exception is ForbiddenAppException fEx)
            {
                _logger.LogError(fEx, $"Forbidden exception.");
                await WriteResponse(httpContext, fEx, ProblemDetailTypes["3.1"], StatusCodes.Status403Forbidden);
            }
            else if (exception is NotFoundAppException nfEx)
            {
                _logger.LogError(nfEx, $"Not found exception.");
                await WriteResponse(httpContext, nfEx, ProblemDetailTypes["6.5.4"], StatusCodes.Status404NotFound);
            }
            else if (exception is UnprocessableEntityAppException ueEx)
            {
                _logger.LogError(ueEx, $"Unprocessable entity exception.");
                await WriteResponse(httpContext, ueEx, ProblemDetailTypes["6.5.4"], StatusCodes.Status422UnprocessableEntity);
            }
            else if (exception is AppException appEx)
            {                
                _logger.LogError(appEx, appEx.Message);
                await WriteResponse(httpContext, appEx, ProblemDetailTypes["6.6.1"], StatusCodes.Status500InternalServerError);
            }
            else if (exception is OperationCanceledException ocEx)
            {
                var title = "Operation cancelled.";
                var ocAppEx = new AppException(title, ocEx.Message, null, ocEx);

                _logger.LogInformation(title);
                await WriteResponse(httpContext, ocAppEx, ProblemDetailTypes["6.6.1"], StatusCodes.Status500InternalServerError);
            }
            else
            {
                var title = "Something went wrong.";
                var otherAppEx = new AppException(title, exception.Message, null, exception);

                _logger.LogError(otherAppEx, title);
                await WriteResponse(httpContext, otherAppEx, ProblemDetailTypes["6.6.1"], StatusCodes.Status500InternalServerError);
            }
        }

        private async Task WriteResponse(HttpContext context, AppException ex, string? type, int statusCode)
        {
            var details = new ProblemDetails()
            {
                Title = ex?.Message,
                Detail = ex?.Detail,
                Type = type,
                Status = statusCode,
                Instance = $"{context.Request.Method} {context.Request.Path.Value}",
            };

            foreach (var extension in ex?.Extensions!)
            {
                details.Extensions.Add(extension!);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(details, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }));
        }
    }
}
