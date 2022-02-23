using API.Wrappers;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new Response(false, ex.Message));
            }
        }
    }
}