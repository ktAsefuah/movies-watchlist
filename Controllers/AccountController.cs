#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieWatchlist.Models;

namespace MovieWatchlist.Controllers
{
    /// <summary>
    /// Handles user authentication: register, login, logout, profile.
    /// Uses ASP.NET Core Identity for password hashing and sign-in management.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser>  _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
        }

        // ── REGISTER ─────────────────────────────────────────────────────────
        // GET /Account/Register
        public IActionResult Register() =>
            User.Identity.IsAuthenticated ? RedirectToAction("Index", "Home") : View();

        // POST /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new IdentityUser
            {
                UserName    = model.Email,
                Email       = model.Email,
                // Store full name in PhoneNumber temporarily (no extra table needed)
                PhoneNumber = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                TempData["Success"] = $"Welcome, {model.FullName}! Your account has been created.";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        // ── LOGIN ────────────────────────────────────────────────────────────
        // GET /Account/Login
        public IActionResult Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password,
                model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        // ── LOGOUT ───────────────────────────────────────────────────────────
        // POST /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        // ── PROFILE ──────────────────────────────────────────────────────────
        // GET /Account/Profile
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            ViewBag.FullName = user?.PhoneNumber ?? user?.Email ?? "User";
            ViewBag.Email    = user?.Email ?? "";
            return View();
        }

        // POST /Account/Profile (update name / password)
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(string fullName, string newPassword, string confirmPassword)
        {
            var user = await _userManager.GetUserAsync(User);
            bool changed = false;

            if (!string.IsNullOrWhiteSpace(fullName) && fullName != user.PhoneNumber)
            {
                user.PhoneNumber = fullName;
                await _userManager.UpdateAsync(user);
                changed = true;
            }

            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                if (newPassword != confirmPassword)
                {
                    ModelState.AddModelError("", "Passwords do not match.");
                    ViewBag.FullName = user.PhoneNumber;
                    ViewBag.Email    = user.Email;
                    return View();
                }
                var token  = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    foreach (var e in result.Errors)
                        ModelState.AddModelError("", e.Description);
                    ViewBag.FullName = user.PhoneNumber;
                    ViewBag.Email    = user.Email;
                    return View();
                }
                changed = true;
            }

            if (changed) TempData["Success"] = "Profile updated successfully.";
            ViewBag.FullName = user.PhoneNumber;
            ViewBag.Email    = user.Email;
            return View();
        }
    }
}
