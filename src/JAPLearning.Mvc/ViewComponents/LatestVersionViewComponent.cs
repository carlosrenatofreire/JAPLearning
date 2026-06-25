using Microsoft.AspNetCore.Mvc;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;

namespace JAPLearning.Mvc.ViewComponents
{
    /// <summary>
    /// Devolve o número da versão mais recente publicada para o badge do topbar.
    /// </summary>
    public class LatestVersionViewComponent : ViewComponent
    {
        private readonly IAppVersionService _service;

        public LatestVersionViewComponent(IAppVersionService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var latest = await _service.GetLatestPublishedAsync();
            return Content(latest?.VersionNumber ?? string.Empty);
        }
    }
}
