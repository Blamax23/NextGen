using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextGen.Dal.Interfaces;
using NextGen.Model;
using NextGen.Dal.Context;
using System.Net.Mail;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using NextGen.Back.Services;
using MimeKit;
using Google.Apis.Services;
using System.Net;

namespace NextGen.Front.Controllers
{
    public class ContactController : Controller
    {
        private readonly IActualiteSrv _actualiteSrv;
        private readonly ISourceSrv _sourceSrv;
        private readonly IUserSrv _userSrv;
        private readonly NextGenDbContext _context;
        private readonly IConfiguration _configuration;

        public ContactController(IActualiteSrv ActualiteSrv, ISourceSrv sourceSrv, IUserSrv userSrv, NextGenDbContext context, IConfiguration configuration)
        {
            _actualiteSrv = ActualiteSrv;
            _sourceSrv = sourceSrv;
            _userSrv = userSrv;
            _context = context;
            _configuration = configuration;
        }
        // GET: ContactController
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(string Email, string Objet, string Contenu)
        {
            try
            {
                string fromMail = _configuration.GetSection("EmailSettings")["EmailSender"];
                string fromPassword = _configuration.GetSection("EmailSettings")["Password"];
                string toMail = _configuration.GetSection("EmailSettings")["EmailReceiver"];
                string smtpServer = _configuration.GetSection("EmailSettings")["Host"];

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromMail);
                mail.To.Add(new MailAddress(toMail));
                mail.Subject = Objet;
                mail.Body = $"<html><body>De : {Email}<br><br>{Contenu}</body></html>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient(smtpServer)
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };

                smtp.Send(mail);
                // Rediriger l'utilisateur vers une page de succès
                return RedirectToAction("SuccessPage", "Contact");
            }
            catch (Exception ex)
            {
                // Gérer les erreurs éventuelles
                ModelState.AddModelError("", "Une erreur s'est produite lors de l'envoi du message : " + ex.Message);
                return View("Contact", new { Email, Objet, Contenu });
            }
        }

        public IActionResult SuccessPage()
        {
            return View();
        }

        // GET: ContactController/Details/5
        public ActionResult DeleteActualite(int id)
        {
            _actualiteSrv.DeleteActualite(id);

            List<Source> sources = _sourceSrv.GetSourcesByActualite(id);
            foreach (var source in sources)
            {
                _context.Sources.Remove(source);
            }
            // Suppression des sources associées à l'actualité dans le dossier Uploads de wwwroot
            foreach (var source in sources)
            {
                var chemin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads", Path.GetFileName(source.Path));

                if (System.IO.File.Exists(chemin))
                {
                    System.IO.File.Delete(chemin);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: ContactController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContactController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ContactController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContactController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
