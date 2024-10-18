using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Dal.Context;
using System.Security.Claims;

namespace NextGen.Front.Controllers
{
    public class LoginController : Controller
    {
        private readonly NextGenDbContext _context;

        public LoginController(NextGenDbContext context)
        {
            _context = context;
        }

        // GET: LoginController
        [HttpGet]
        public ActionResult Connect()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Connect(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("IsAdmin", user.IsAdmin.ToString()),
                        new Claim("Id", user.Id.ToString())
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // Pour rendre l'authentification persistante
                };

                // SignInAsync va créer le cookie d'authentification
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Connect");
        }
    }
}
