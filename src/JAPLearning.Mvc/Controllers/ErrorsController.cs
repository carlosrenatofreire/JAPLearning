using System.Security.Claims;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;

namespace JAPLearning.Mvc.Controllers
{
    public class ErrorsController : Controller
    {
        private readonly IAuditLogService _auditLog;

        public ErrorsController(IAuditLogService auditLog)
        {
            _auditLog = auditLog;
        }

        [Route("Errors/{statusCode:int}")]
        public async Task<IActionResult> Handle(int statusCode)
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            var reExecuteFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            var originalPath = exceptionFeature?.Path
                            ?? reExecuteFeature?.OriginalPath
                            ?? Request.Path.Value
                            ?? "/";

            var currentUser  = User.FindFirstValue(ClaimTypes.Email) ?? "anonymous";
            var exception    = exceptionFeature?.Error;

            ViewBag.StatusCode   = statusCode;
            ViewBag.OriginalPath = originalPath;
            ViewBag.Exception    = exception;

            // Regista na auditoria apenas erros relevantes
            await TryLogAsync(statusCode, currentUser, originalPath, exception);

            return statusCode switch
            {
                401 => View("Error401"),
                403 => View("Error403"),
                404 => View("Error404"),
                _   => View("Error500")
            };
        }

        private async Task TryLogAsync(int statusCode, string user, string path, Exception? ex)
        {
            try
            {
                // 404 de assets estáticos (js, css, png…) — ignora para não poluir a auditoria
                if (statusCode == 404)
                {
                    var ext = Path.GetExtension(path).ToLowerInvariant();
                    if (ext is ".js" or ".css" or ".png" or ".jpg" or ".ico" or ".svg" or ".woff" or ".woff2" or ".map")
                        return;
                }

                var message = ex != null
                    ? $"[{statusCode}] {ex.Message} — {path}"
                    : $"[{statusCode}] {path}";

                await _auditLog.LogErrorAsync(
                    createdBy:      user,
                    message:        message,
                    httpStatusCode: statusCode,
                    stackTrace:     ex?.ToString());
            }
            catch { /* falha no log nunca pode propagar */ }
        }
    }
}
