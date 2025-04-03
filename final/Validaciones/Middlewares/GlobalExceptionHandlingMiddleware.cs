using Entidades.Modelos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validaciones.Excepciones;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Validaciones.Middlewares
{
    public class GlobalExceptionHandlingMiddleware(RequestDelegate next)
    {

        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {   //Para indicar que la respuesta va a ser de tipo json

                await HandleGlobalExceptionAsync(context, error);
            }
        }


        private static Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var codigo = StatusCodes.Status500InternalServerError;

            switch (exception)
            {
                case UnauthenticatedException ex:  //Cuando intenta hacer login y no coinciden datos
                    codigo = ex.GetStatusCode();
                    break;                                              
                case AccessDeniedException ex:     //Cuando el usuario no tiene suficientes permisos para acceder a un recurso
                    codigo = ex.GetStatusCode();
                    break;
                case InvalidRequestException ex:     //Cuando el usuario envia un pedido con formato invalido
                    codigo = ex.GetStatusCode();
                    break;
                case ItemNotFoundException ex:     //Cuando el usuario no encuentre algun objeto por id
                    codigo = ex.GetStatusCode();
                    break;
                case ServiceNotAvailableException ex:     //Cuando el servicio no este disponible
                    codigo = ex.GetStatusCode();
                    break;
                case ConflictException ex:     //Cuando el usuario se encuentre con un conflicto
                    codigo = ex.GetStatusCode();
                    break;
                default:  //es el manejo de un error general. 
                    GeneralApiExcepcion gral;
                    break;

            }
          
            return context.Response.WriteAsync(JsonConvert.SerializeObject( new ResponseError() 
            { 
                Succeeded = false,
                StatusCode = codigo,
                Message = exception?.Message, 
                StackTrace = exception?.StackTrace

            }));


        }
    }

}

