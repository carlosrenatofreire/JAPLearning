using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using System.Diagnostics;
using System.Security.Claims;

namespace JAPLearning.Mvc.Middlewares
{
    /// <summary>
    /// Regista na auditoria os pedidos HTTP significativos:
    /// POST / PUT / DELETE de utilizadores autenticados.
    /// Pedidos de assets estáticos e GETs anónimos são ignorados.
    /// </summary>
    public class RequestLoggingMiddleware(RequestDelegate next)
    {
        private static readonly HashSet<string> _skipExtensions = new(StringComparer.OrdinalIgnoreCase)
        {
            ".js", ".css", ".png", ".jpg", ".jpeg", ".svg",
            ".ico", ".woff", ".woff2", ".ttf", ".map", ".webp"
        };

        private static readonly HashSet<string> _loggableMethods = new(StringComparer.OrdinalIgnoreCase)
        {
            "POST", "PUT", "DELETE", "PATCH"
        };

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            await next(context);
            sw.Stop();

            // Apenas pedidos que alteram estado + utilizador autenticado
            if (!context.User.Identity?.IsAuthenticated == true) return;
            if (!_loggableMethods.Contains(context.Request.Method)) return;

            var path = context.Request.Path.Value ?? string.Empty;
            if (_skipExtensions.Contains(Path.GetExtension(path))) return;

            // Não registar o próprio logout (já coberto pelo AuthenticationAudit)
            if (path.Contains("/Account/Logout", StringComparison.OrdinalIgnoreCase)) return;

            try
            {
                var auditLog = context.RequestServices.GetService<IAuditLogService>();
                if (auditLog == null) return;

                var user       = context.User.FindFirstValue(ClaimTypes.Email) ?? "unknown";
                var method     = context.Request.Method;
                var statusCode = context.Response.StatusCode;
                var duration   = sw.ElapsedMilliseconds;
                var message    = $"{method} {path} → {statusCode} ({duration}ms)";

                if (statusCode >= 500)
                    await auditLog.LogErrorAsync(user, message, statusCode);
                else if (statusCode >= 400)
                    await auditLog.LogErrorAsync(user, message, statusCode);
                else
                    await auditLog.LogInfoAsync(user, $"Http{method}", ExtractEntity(path), message);
            }
            catch
            {
                // Falha no log não pode interromper o request
            }
        }

        /// <summary>
        /// Extrai o nome da entidade a partir do path (ex: /Topics/Create → Topic).
        /// </summary>
        private static string ExtractEntity(string path)
        {
            var segments = path.Trim('/').Split('/');
            return segments.Length > 0 ? segments[0] : "Unknown";
        }
    }
}
