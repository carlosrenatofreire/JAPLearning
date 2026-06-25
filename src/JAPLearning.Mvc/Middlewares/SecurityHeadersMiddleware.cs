namespace JAPLearning.Mvc.Middlewares
{
    /// <summary>
    /// Adiciona cabeçalhos de segurança HTTP a todos os responses.
    /// Protege contra clickjacking, MIME sniffing, XSS e restringe permissões do browser.
    /// </summary>
    public class SecurityHeadersMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            var headers = context.Response.Headers;

            // Impede que a página seja carregada em iframe (clickjacking)
            headers["X-Frame-Options"] = "SAMEORIGIN";

            // Impede que o browser adivinhe o tipo de conteúdo (MIME sniffing)
            headers["X-Content-Type-Options"] = "nosniff";

            // Activa o filtro XSS embutido nos browsers mais antigos
            headers["X-XSS-Protection"] = "1; mode=block";

            // Controla a informação de referrer enviada em navigações
            headers["Referrer-Policy"] = "strict-origin-when-cross-origin";

            // Desactiva funcionalidades do browser que não são necessárias
            headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=(), payment=()";

            // Força HTTPS e guarda a preferência por 1 ano (só em HTTPS)
            if (context.Request.IsHttps)
                headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";

            await next(context);
        }
    }
}
