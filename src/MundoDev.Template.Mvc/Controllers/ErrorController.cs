using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace MundoDev.Template.Mvc.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/Index/{statusCode?}")]
        public IActionResult Index(int? statusCode)
        {
            // If no status code provided, try to get from exception handler
            if (statusCode == null || statusCode == 0)
            {
                var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
                statusCode = exceptionFeature != null ? 500 : 404;
            }

            ViewData["StatusCode"] = statusCode;

            ViewData["Title"] = statusCode switch
            {
                401 => "Não Autorizado",
                403 => "Acesso Negado",
                404 => "Página Não Encontrada",
                500 => "Erro Interno",
                _   => "Erro"
            };

            return View();
        }
    }
}
