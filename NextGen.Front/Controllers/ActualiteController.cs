using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NextGen.Dal.Interfaces;
using NextGen.Model;
using NextGen.Dal.Context;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        public async Task<ActionResult> Index()
        {
            var instagramPreview = new InstagramPreview("IGQWRQMkhLWTZAYeUxaeHZA3bVN6SjRTVFdOa0xlYmtDSGFNeTg5ZA0xNWnljUDNQUFE4YlE1YTYwVkFVdnVpVDFBWHhURUNSVkkwMzNoandpbXpkVThVbl84WUc4YWIzLU5nQlAxbU5FVkI2NWVPRGd6YWtiV3ZAENW8ZD", "17841418155900065");
            //var profile = await instagramPreview.GetProfilePreviewAsync();

            ActualitesWithSourceAndIgProfile actualitesGlobales = new ActualitesWithSourceAndIgProfile();
            var actualites = _actualiteSrv.GetAllActualites();
            foreach (var actualite in actualites)
            {
                var actualiteWithSource = new ActualiteWithSource
                {
                    Id = actualite.Id,
                    Actualite = actualite,
                    Source = _sourceSrv.GetSourcesByActualite(actualite.Id),
                    User = _userSrv.GetUser(actualite.IdUtilisateur),
                };
                actualiteWithSource.InsertLinks();
                actualitesGlobales.Actualites.Add(actualiteWithSource);
            }

            //actualitesGlobales.InstagramProfile = profile;
            return View(actualitesGlobales);
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
    }

    public class InstagramPreview
    {
        private readonly string _accessToken;
        private readonly string _userId;
        private readonly HttpClient _httpClient;

        public InstagramPreview(string accessToken, string userId)
        {
            _accessToken = accessToken;
            _userId = userId;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.instagram.com/")
            };
        }

        public async Task<InstagramProfile> GetProfilePreviewAsync()
        {
            try
            {
                // Récupérer les informations du profil
                var profileResponse = await _httpClient.GetAsync(
                    $"v12.0/{_userId}?fields=username,profile_picture_url,followers_count&access_token={_accessToken}");
                profileResponse.EnsureSuccessStatusCode();
                var profileDataString = await profileResponse.Content.ReadAsStringAsync();
                var profileData = JsonSerializer.Deserialize<InstagramProfile>(profileDataString);

                // Récupérer les posts récents
                var mediaResponse = await _httpClient.GetAsync(
                    $"v12.0/{_userId}/media?fields=id,media_url,caption,permalink,media_type&limit=9&access_token={_accessToken}");
                mediaResponse.EnsureSuccessStatusCode();
                var mediaDataString = await mediaResponse.Content.ReadAsStringAsync();
                var mediaData = JsonSerializer.Deserialize<InstagramMediaResponse>(mediaDataString);

                var posts = new List<InstagramPost>();
                foreach (var media in mediaData.Data)
                {
                    posts.Add(new InstagramPost
                    {
                        Id = media.Id,
                        MediaUrl = media.MediaUrl,
                        Caption = media.Caption,
                        Permalink = media.Permalink,
                        MediaType = media.MediaType
                    });
                }

                return new InstagramProfile
                {
                    Username = profileData.Username,
                    FollowersCount = profileData.FollowersCount,
                    ProfilePictureUrl = profileData.ProfilePictureUrl,
                    RecentPosts = posts
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des données Instagram", ex);
            }
        }
    }
}
