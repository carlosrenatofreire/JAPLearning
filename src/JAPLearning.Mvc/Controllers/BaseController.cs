using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Internals.Shareds;

namespace JAPLearning.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificator _notificator;

        protected BaseController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool IsOperationValid() => !_notificator.HasNotifications;

        /// <summary>
        /// Adiciona as notificações ao ModelState como erros (bloco vermelho).
        /// Usar para erros de sistema / falhas inesperadas.
        /// </summary>
        protected void AddErrors()
        {
            foreach (var notification in _notificator.GetNotifications())
                ModelState.AddModelError(string.Empty, notification.Message);
        }

        /// <summary>
        /// Passa as notificações para TempData como avisos (bloco amarelo).
        /// Usar para violações de regras de negócio / validações FluentValidation.
        /// </summary>
        protected void AddWarnings()
        {
            var messages = _notificator.GetNotifications()
                                       .Select(n => n.Message)
                                       .ToList();
            if (messages.Any())
                TempData["ValidationWarnings"] = string.Join("||", messages);
        }
    }
}
