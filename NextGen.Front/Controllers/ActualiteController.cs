using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextGen.Dal.Interfaces;
using NextGen.Model;
using NextGen.Dal.Context;

namespace NextGen.Front.Controllers
{
    public class ActualiteController : Controller
    {
        private readonly IActualiteSrv _actualiteSrv;
        private readonly ISourceSrv _sourceSrv;
        private readonly IUserSrv _userSrv;
        private readonly NextGenDbContext _context;

        public ActualiteController(IActualiteSrv ActualiteSrv, ISourceSrv sourceSrv, IUserSrv userSrv, NextGenDbContext context)
        {
            _actualiteSrv = ActualiteSrv;
            _sourceSrv = sourceSrv;
            _userSrv = userSrv;
            _context = context;
        }
        // GET: ActualiteController
        public ActionResult Index()
        {

            List<ActualiteWithSource> actualiteWithSources = new List<ActualiteWithSource>();
            var actualites = _actualiteSrv.GetAllActualites();

            foreach (var actualite in actualites)
            {
                var actualiteWithSource = new ActualiteWithSource
                {
                    Actualite = actualite,
                    Source = _sourceSrv.GetSourcesByActualite(actualite.Id),
                    User = _userSrv.GetUser(actualite.IdUtilisateur)
                };
                actualiteWithSource.InsertLinks();
                actualiteWithSources.Add(actualiteWithSource);
            }
            return View(actualiteWithSources);
        }

        [HttpPost]
        public IActionResult AddActualite(string Titre, string Contenu, IFormFile[] Sources)
        {
            Int32.TryParse(User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value, out int result);
            // Logique pour ajouter une nouvelle actualité
            var nouvelleActualite = new Actualite
            {
                Titre = Titre,
                Contenu = Contenu,
                DateModification = DateTime.Now,
                IdUtilisateur = result
            };

            _actualiteSrv.AddActualite(nouvelleActualite);

            // Logique pour gérer les fichiers sources (images, vidéos, etc.)
            foreach (var source in Sources)
            {
                if (source != null && source.Length > 0)
                {
                    var chemin = "~/Uploads/" + source.FileName;
                    var cheminCopy = "wwwroot/Uploads/" + source.FileName;
                    using (var stream = new FileStream(cheminCopy, FileMode.Create))
                    {
                        source.CopyTo(stream);
                    }

                    var extension = Path.GetExtension(source.FileName).Substring(1);
                    string typeString = "";
                    switch (extension.ToLower())
                    {
                        case "jpg":
                        case "png":
                            typeString = "image";
                            break;
                        case "mp4":
                            typeString = "video";
                            break;
                        case "pdf":
                            typeString = "pdf";
                            break;
                        case "mp3":
                            typeString = "audio";
                            break;
                        default:
                            typeString = "link";
                            break;
                    }

                    var nouvelleSource = new Source
                    {
                        Path = chemin,
                        Type = typeString,
                        Name = source.FileName,
                        IdActualite = nouvelleActualite.Id
                    };

                    _context.Sources.Add(nouvelleSource);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");  // Retour à la page des actualités après soumission
        }

        // GET: ActualiteController/Details/5
        public ActionResult DeleteActualite(int id)
        {
            _actualiteSrv.DeleteActualite(id);

            return RedirectToAction("Index");
        }

        // GET: ActualiteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActualiteController/Create
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

        // GET: ActualiteController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ActualiteController/Edit/5
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

        // GET: ActualiteController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ActualiteController/Delete/5
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
