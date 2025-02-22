using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validaciones.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next)
    {

        private readonly RequestDelegate _next = next;


        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";

                if (ex is ArgumentException || ex is ArgumentNullException)
                {
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
                //else if (ex is UsuarioYaRegistradoException)
                //{
                //    context.Response.StatusCode = StatusCodes.Status409Conflict;
                //}
                else if (ex is InvalidOperationException)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                }
                else if (ex is KeyNotFoundException)
                {
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                }

                var errorResponse = new { mensaje = ex.Message };
                //await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }

}

