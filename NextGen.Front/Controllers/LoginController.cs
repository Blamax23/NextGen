using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NextGen.Dal.Context;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            // On hash le password pour le comparer à celui en base avec SHA256
            password = HashPassword(password);
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.Password == password);
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
            return RedirectToAction("Connect", "Login");
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
