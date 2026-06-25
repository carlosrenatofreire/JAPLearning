using JAPLearning.Mvc.Middlewares;

namespace JAPLearning.Mvc.Configurations
{
    public static class WebAppConfig
    {
        public static WebApplication UseWebAppConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                // Em desenvolvimento mostra o stack trace completo no browser
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // 1. Global Exception — apenas em produção
                app.UseMiddleware<GlobalExceptionMiddleware>();
            }

            if (!app.Environment.IsDevelopment())
                app.UseHsts();

            // 2. Security Headers — antes de qualquer resposta
            app.UseMiddleware<SecurityHeadersMiddleware>();

            // Páginas de erro personalizadas para 401, 403, 404, etc.
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");

            app.UseHttpsRedirection();
            app.UseRouting();

            // 3. Rate Limiting — deve vir APÓS UseRouting para ler metadados do endpoint
            app.UseRateLimiter();

            app.UseAuthentication();
            app.UseAuthorization();

            // 4. Request Logging — após autenticação para ter o utilizador disponível
            app.UseMiddleware<RequestLoggingMiddleware>();

            // 5. AuthenticationAudit — via eventos de cookie em AuthConfig.cs

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            return app;
        }
    }
}
