using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using JAPLearning.Business.Interfaces.Internals.Shareds;
using JAPLearning.Business.Interfaces.Services.Auxiliaries;
using JAPLearning.Business.Interfaces.Services.Entities;
using JAPLearning.Mvc.ViewModels.Account;
using System.Security.Claims;

namespace JAPLearning.Mvc.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuditLogService _auditLog;

        public AccountController(IUserService userService, IAuditLogService auditLog,
            INotificator notificator) : base(notificator)
        {
            _userService = userService;
            _auditLog    = auditLog;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnableRateLimiting("login")]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.GetByEmailAsync(model.Email);

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "—";

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                try { await _auditLog.LogWarningAsync(model.Email, "LoginFailed",
                    "User", $"Credenciais inválidas | IP: {ip}"); } catch { }
                ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos.");
                return View(model);
            }

            if (!user.IsActived)
            {
                try { await _auditLog.LogWarningAsync(model.Email, "LoginFailed",
                    "User", $"Conta inactiva | IP: {ip}"); } catch { }
                ModelState.AddModelError(string.Empty, "Conta inativa. Contacte o administrador.");
                return View(model);
            }

            // TODO: If Role navigation property is not loaded, user.Role may be null.
            // Consider updating IUserRepository.GetByEmail to include Role navigation.
            var roleName = user.Role?.Name ?? string.Empty;

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, roleName),
                new("PhotoUrl", user.PhotoUrl ?? string.Empty),
                new("TeamId", user.TeamId.ToString())
            };

            var identity  = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProps = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc   = model.RememberMe
                    ? DateTimeOffset.UtcNow.AddDays(30)
                    : DateTimeOffset.UtcNow.AddHours(24)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProps);

            // ── Login tracking (apenas Alunos) ──────────────────────────
            if (roleName == "Aluno")
                await _userService.RecordLoginAsync(user.Id);

            // ── Obrigar troca de senha (todos os roles) ──────────────────
            if (user.MustChangePassword)
                return RedirectToAction(nameof(ChangePassword));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return roleName == "Aluno"
                ? RedirectToAction("Dashboard", "Student")
                : RedirectToAction("Index", "Home");
        }

        // ── Troca de senha obrigatória (primeiro acesso — todos os roles) ─

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var user   = await _userService.GetByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Utilizador não encontrado.");
                return View(model);
            }

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.Password))
            {
                ModelState.AddModelError(nameof(model.CurrentPassword), "Senha actual incorrecta.");
                return View(model);
            }

            var newHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            var ok = await _userService.ChangePasswordAsync(user.Id, newHash);
            if (!ok)
            {
                AddErrors();
                return View(model);
            }

            TempData["Success"] = "Senha alterada com sucesso! Bem-vindo à plataforma.";

            var roleName = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty;
            return roleName == "Aluno"
                ? RedirectToAction("Dashboard", "Student")
                : RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();
    }
}
