using System;
using Incidentes.Excepciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Incidentes.WebApi.Filters
{
    public class TrapExcepciones : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            int statusCode = 500;
            string content = context.Exception.Message;

            var type = context.Exception.GetType();
            if (type == typeof(ExcepcionAccesoNoAutorizado))
            {
                statusCode = 401; 
            }
            else if (type == typeof(ExcepcionArgumentoNoValido))
            {
                statusCode = 422; 
            }
            else if (type == typeof(ExcepcionElementoNoExiste))
            {
                statusCode = 409; 
            }
            else if (type == typeof(ExcepcionLargoTexto))
            {
                statusCode = 422; 
            }
            else
            {
                content = "Internal Server Error | " + context.Exception.Message;
            }

            context.Result = new ContentResult()
            {
                StatusCode = statusCode,
                Content = content
            };
        }
    }
}
