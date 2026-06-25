using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using System.Security.Claims;

namespace JAPLearning.Mvc.Middlewares
{
    /// <summary>
    /// Captura excepções não tratadas em qualquer ponto do pipeline,
    /// regista na auditoria com stack trace e redireciona para a página de erro 500.
    /// Substitui/complementa o UseExceptionHandler nativo.
    /// </summary>
    public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Excepção não tratada: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            try
            {
                var auditLog = context.RequestServices.GetService<IAuditLogService>();
                if (auditLog != null)
                {
                    var user = context.User.FindFirstValue(ClaimTypes.Email) ?? "anonymous";
                    await auditLog.LogErrorAsync(user, ex.Message, 500, ex.ToString());
                }
            }
            catch
            {
                // Falha no log não deve impedir o redirect de erro
            }

            if (!context.Response.HasStarted)
            {
                context.Response.Redirect("/Errors/500");
            }
        }
    }
}
