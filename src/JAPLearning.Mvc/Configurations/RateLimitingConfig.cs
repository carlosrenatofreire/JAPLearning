using System.Threading.RateLimiting;

namespace JAPLearning.Mvc.Configurations
{
    public static class RateLimitingConfig
    {
        /// <summary>
        /// Políticas de rate limiting particionadas por IP:
        /// - "login"   : máx. 5 tentativas por IP a cada 5 minutos (anti-brute-force)
        /// - "general" : máx. 120 pedidos por IP por minuto (protecção geral)
        /// </summary>
        public static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = 429;

                // ── Anti-brute-force para login (por IP + email) ────────
                options.AddPolicy("login", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    // Lê o email do form body para criar uma chave por utilizador+IP.
                    // Assim cada utilizador tem o seu próprio contador independente.
                    var email = httpContext.Request.HasFormContentType
                        ? httpContext.Request.Form["Email"].ToString().Trim().ToLowerInvariant()
                        : string.Empty;

                    var partitionKey = string.IsNullOrEmpty(email)
                        ? ip
                        : $"{ip}:{email}";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: partitionKey,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit         = 5,
                            Window              = TimeSpan.FromMinutes(5),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit          = 0
                        });
                });

                // ── Protecção geral por IP ──────────────────────────────
                options.AddPolicy("general", httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit         = 120,
                            Window              = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit          = 10
                        }));

                // Handler para resposta quando o limite é excedido
                options.OnRejected = (context, cancellationToken) =>
                {
                    var isLogin = context.HttpContext.Request.Path
                        .StartsWithSegments("/Account/Login", StringComparison.OrdinalIgnoreCase);

                    if (isLogin)
                    {
                        // Redireciona de volta ao login com mensagem de bloqueio em TempData
                        var tempData = context.HttpContext.RequestServices
                            .GetRequiredService<Microsoft.AspNetCore.Mvc.ViewFeatures.ITempDataDictionaryFactory>()
                            .GetTempData(context.HttpContext);

                        tempData["RateLimitError"] =
                            "Demasiadas tentativas de login falhadas. Por segurança, o acesso foi bloqueado temporariamente. Aguarde 5 minutos e tente novamente.";
                        tempData.Save();

                        context.HttpContext.Response.Redirect("/Account/Login");
                    }
                    else
                    {
                        context.HttpContext.Response.StatusCode = 429;
                        context.HttpContext.Response.Redirect("/Errors/429");
                    }

                    return ValueTask.CompletedTask;
                };
            });

            return services;
        }
    }
}
