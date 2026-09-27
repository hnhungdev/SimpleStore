using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.ExceptionHandlers;

public sealed class CustomExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext context,
        Exception exception,
        CancellationToken ct)
    {
        ProblemDetails problemDetails;

        switch (exception)
        {
            case ValidationException ex:

                var validationErrors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g
                            .Select(x => x.ErrorMessage)
                            .ToArray());

                var validationProblem =
                    new ValidationProblemDetails(validationErrors)
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Title = "Validation failed",
                        Instance = context.Request.Path
                    };

                context.Response.StatusCode = validationProblem.Status!.Value;

                await context.Response.WriteAsJsonAsync(validationProblem, ct);

                return true;

            case NotFoundException ex:

                problemDetails = new ProblemDetails
                {
                    Title = "Resource not found",
                    Detail = ex.Message,
                    Status = StatusCodes.Status404NotFound,
                    Instance = context.Request.Path
                };

                problemDetails.Extensions["errors"] = ex.Details;

                break;

            case ClientValidationException ex:

                problemDetails = new ProblemDetails
                {
                    Title = "Client validation failed",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Instance = context.Request.Path
                };

                problemDetails.Extensions["errors"] = ex.Details;

                break;

            case DomainException ex:

                problemDetails = new ProblemDetails
                {
                    Title = "Business rule violation",
                    Detail = ex.Message,
                    Status = StatusCodes.Status400BadRequest,
                    Instance = context.Request.Path
                };

                break;

            default:

                problemDetails = new ProblemDetails
                {
                    Title = "Internal server error",
                    Detail = "An unexpected error occurred.",
                    Status = StatusCodes.Status500InternalServerError,
                    Instance = context.Request.Path
                };

                break;
        }

        context.Response.StatusCode = problemDetails.Status ?? 500;

        await context.Response.WriteAsJsonAsync(problemDetails, ct);

        return true;
    }
}