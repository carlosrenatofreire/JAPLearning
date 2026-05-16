using Microsoft.AspNetCore.Mvc;
using MundoDev.Business.Interfaces.Internals.Shareds;

namespace MundoDev.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificator _notificator;

        protected BaseController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool IsOperationValid() => !_notificator.HasNotifications;

        protected void AddErrors()
        {
            foreach (var notification in _notificator.GetNotifications())
                ModelState.AddModelError(string.Empty, notification.Message);
        }
    }
}
