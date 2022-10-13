using EnvioFacturaSMS.Applications.Excepetions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace EnvioFacturaSMS.Aplications.Middleware
{
    public class ErrorHandlerMiddleware : ExceptionFilterAttribute
    {
        private const string MensajeGenericoError = "Ha ocurrido un error";

        public ErrorHandlerMiddleware()
        {
        }

        public override void OnException(ExceptionContext Context)
        {
            Context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            string Message = Context.Exception.Message;

            if (Context.Exception is ApiAbonoException)
            {
                Context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                SaveLog();

            }else if (Context.Exception is AzureStorageException)
            {
                Context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                SaveLog();
            }
            else
            {
                Context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                Message = MensajeGenericoError;
            }

            var Response = new
            {
                Message,
                TypeException = Context.Exception.GetType().ToString()
            };

            Context.Result = new ObjectResult(Response);

        }

        public void SaveLog()
        {

        }


    }
}
