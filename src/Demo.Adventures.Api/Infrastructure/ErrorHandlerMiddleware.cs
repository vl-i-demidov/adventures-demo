using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Demo.Adventures.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Adventures.Api.Infrastructure
{
    /// <summary>
    ///     Provides custom exception handling logic.
    /// </summary>
    internal class ErrorHandlerMiddleware
    {
        private readonly bool _includeDetails;
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next, bool includeDetails)
        {
            _next = next;
            _includeDetails = includeDetails;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var statusCode = ResolveStatusCode(ex);

                var response = context.Response;
                response.ContentType = "application/problem+json";
                response.StatusCode = statusCode;

                // Get the details to display, depending on whether we want to expose the raw exception
                var title = _includeDetails ? ex.Message : "An error occurred";
                var details = _includeDetails ? ex.ToString() : null;

                var problem = new ProblemDetails
                {
                    Status = statusCode,
                    Title = title,
                    Detail = details
                };

                var result = JsonSerializer.Serialize(problem);
                await response.WriteAsync(result);
            }
        }

        private static int ResolveStatusCode(Exception ex)
        {
            return ex is ServiceException se ? (int)se.Status : (int)HttpStatusCode.InternalServerError;
        }
    }
}