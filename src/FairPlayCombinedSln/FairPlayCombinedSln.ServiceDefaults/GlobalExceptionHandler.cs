using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombinedSln.ServiceDefaults
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
                long? errorId = default;
                try
                {
                    FairPlayCombinedDbContext fairPlayCombinedDbContext =
                    httpContext.RequestServices.GetRequiredService<FairPlayCombinedDbContext>();
                    ErrorLog errorLog = new()
                    {
                        FullException = exception.ToString(),
                        StackTrace = exception.StackTrace,
                        Message = exception.Message
                    };
                    await fairPlayCombinedDbContext.ErrorLog.AddAsync(errorLog);
                    await fairPlayCombinedDbContext.SaveChangesAsync();
                    errorId = errorLog.ErrorLogId;
                }
                catch (Exception)
                {
                    //Global exception, not rethrowing so server app does not crash
                }
                ProblemDetails problemDetails = new ProblemDetails();
                if (exception is RuleException || exception is ValidationException)
                {
                    problemDetails.Detail = exception.Message;
                }
                else
                {
                    string userVisibleError = "An error ocurred.";
                    if (errorId.GetValueOrDefault(0) > 0)
                    {
                        userVisibleError += $" Error code: {errorId}";
                    }
                    problemDetails.Detail = userVisibleError;
                }
                problemDetails.Status = (int)System.Net.HttpStatusCode.BadRequest;
                await httpContext.Response.WriteAsJsonAsync<ProblemDetails>(problemDetails);
            return true;
        }
    }
}
