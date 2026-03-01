namespace DungeonExplorer.Api.Filters;

public class ExceptionFilter(ILogger<ExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Log the exception details
        logger.LogError(context.Exception, "Unhandled exception occurred");

        var result = new ObjectResult(new
        {
            error = "An unexpected error occurred",
            details = "Please try again later."
        })
        {
            StatusCode = 500
        };

        context.Result = result;
        context.ExceptionHandled = true;
    }
}