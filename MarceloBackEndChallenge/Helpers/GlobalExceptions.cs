using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public static class GlobalExceptions
{
    public static void AddGlobalExceptions(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(applicationBuilder =>
        {
            applicationBuilder.Run(async contexto =>
            {
                contexto.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
 
                var erroNoRequest = contexto.Features.Get<IExceptionHandlerFeature>();
 
                if (erroNoRequest != null)
                {
                    var detalheProblema = new ProblemDetails
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Type   = "Erro",
                        Title  = "Erro no servidor",
                        Detail = erroNoRequest.Error.Message
                    };
 
                    await contexto.Response.WriteAsJsonAsync(detalheProblema);
                }
            });
        });
    }
}