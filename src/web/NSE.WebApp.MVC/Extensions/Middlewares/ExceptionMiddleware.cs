using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (CustomHttpRequestException ex)
            {

                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (ValidationApiException ex) //Refit Exception
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch(ApiException ex)
            {
                HandleRequestExceptionAsync(httpContext, ex.StatusCode);
            }
            catch (BrokenCircuitException)
            {
                HandleCircuitBreakerException(httpContext);
            }
        }

        private static void HandleRequestExceptionAsync(HttpContext httpContext, HttpStatusCode statusCode)
        {
            if(statusCode == HttpStatusCode.Unauthorized)
            {
                httpContext.Response.Redirect($"/login?ReturnUrl={httpContext.Request.Path}");
                
                return;
            }

            httpContext.Response.StatusCode = (int)statusCode;
        }

        public static void HandleCircuitBreakerException(HttpContext context)
        {
            context.Response.Redirect("/system-unavailable");
        }
    }
}
