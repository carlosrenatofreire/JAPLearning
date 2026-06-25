using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace JAPLearning.Mvc.Configurations
{
    public static class AuthConfig
    {
        public static IServiceCollection AddAuthConfiguration(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath         = "/Account/Login";
                    options.LogoutPath        = "/Account/Logout";
                    options.AccessDeniedPath  = "/Account/AccessDenied";
                    options.ExpireTimeSpan    = TimeSpan.FromHours(8);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly   = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;

                    // ── Auditoria de autenticação ───────────────────────
                    options.Events = new CookieAuthenticationEvents
                    {
                        // Login bem-sucedido
                        OnSignedIn = async ctx =>
                        {
                            try
                            {
                                var auditLog = ctx.HttpContext.RequestServices
                                    .GetService<IAuditLogService>();
                                if (auditLog == null) return;

                                var email = ctx.Principal?.FindFirstValue(ClaimTypes.Email) ?? "unknown";
                                var role  = ctx.Principal?.FindFirstValue(ClaimTypes.Role) ?? "—";
                                var ip    = ctx.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "—";

                                await auditLog.LogInfoAsync(email, "Login",
                                    "User", $"Login bem-sucedido | Perfil: {role} | IP: {ip}");
                            }
                            catch { /* log não pode bloquear o login */ }
                        },

                        // Sessão terminada (logout)
                        OnSigningOut = async ctx =>
                        {
                            try
                            {
                                var auditLog = ctx.HttpContext.RequestServices
                                    .GetService<IAuditLogService>();
                                if (auditLog == null) return;

                                var email = ctx.HttpContext.User
                                    .FindFirstValue(ClaimTypes.Email) ?? "unknown";
                                var ip = ctx.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "—";

                                await auditLog.LogInfoAsync(email, "Logout",
                                    "User", $"Sessão terminada | IP: {ip}");
                            }
                            catch { /* log não pode bloquear o logout */ }
                        },

                        // Acesso negado (403)
                        OnRedirectToAccessDenied = async ctx =>
                        {
                            try
                            {
                                var auditLog = ctx.HttpContext.RequestServices
                                    .GetService<IAuditLogService>();
                                if (auditLog == null) return;

                                var email = ctx.HttpContext.User
                                    .FindFirstValue(ClaimTypes.Email) ?? "anonymous";
                                var path = ctx.HttpContext.Request.Path.Value ?? "—";
                                var ip   = ctx.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "—";

                                await auditLog.LogErrorAsync(email,
                                    $"Acesso negado a {path} | IP: {ip}", 403);
                            }
                            catch { }

                            ctx.HttpContext.Response.Redirect(ctx.RedirectUri);
                        }
                    };
                });

            return services;
        }
    }
}
