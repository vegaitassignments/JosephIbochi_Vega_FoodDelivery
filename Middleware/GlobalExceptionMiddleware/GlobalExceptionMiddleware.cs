
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Middleware.GlobalExceptionMiddleware;

public class GlobalExceptionMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(ILogger<GlobalExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try {
            await next(context);
        }
        catch (FoodDeliveryNotFoundException e)
        {
            _logger.LogError(e, "An unexpected error occurred: {Message}", e.Message);

             await GenerateProblemDetails(
                e.Message,
                (int)HttpStatusCode.NotFound,
                "NotFound Error",
                context
            );
        }

        catch (FoodDeliveryBadRequestException e)
        {
            await GenerateProblemDetails(
                e.Message,
                (int)HttpStatusCode.BadRequest,
                "Bad Request Error",
                context
            );

        }
        catch (FoodDeliveryUnAuthorizedException e)
        {
            await GenerateProblemDetails(
                e.Message,
                (int)HttpStatusCode.Unauthorized,
                "Unauthorized Error",
                context
            );

        }
        catch (FoodDeliveryForbiddenException e)
        {
            await GenerateProblemDetails(
                e.Message,
                (int)HttpStatusCode.Forbidden,
                "Forbidden Error",
                context
            );

        }
        catch (FoodDeliveryServiceNotFound e)
        {
            await GenerateProblemDetails(
                e.Message,
                (int)HttpStatusCode.InternalServerError,
                "Internal server Error",
                context
            );

        }
        catch (Exception e)
        {
            await GenerateProblemDetails(
                e.Message,
                (int)HttpStatusCode.InternalServerError,
                "Internal server Error",
                context
            );
            
        }
    }

    private async Task  GenerateProblemDetails(string errorMessage, int statusCode, string title, HttpContext context)
    {
        context.Response.StatusCode = statusCode;

        ProblemDetails problemDetails = new(){
                Status = statusCode,
                Title = title,
                Detail = errorMessage,
        };

        string json = JsonSerializer.Serialize(problemDetails);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(json);
    }
}