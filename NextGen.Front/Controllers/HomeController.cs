using Microsoft.AspNetCore.Mvc;
using NextGen.Front.Models;
using NextGen.Model;
using System.ComponentModel.Design;
using System.Diagnostics;
using NextGen.Dal.Interfaces;

namespace NextGen.Front.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IActualiteSrv _ActualiteSrv;
        private readonly IUserSrv _userSrv;
        private readonly ISourceSrv _sourceSrv;

        public HomeController(ILogger<HomeController> logger, IActualiteSrv ActualiteSrv, IUserSrv userSrv, ISourceSrv sourceSrv)
        {
            _logger = logger;
            _ActualiteSrv = ActualiteSrv;
            _userSrv = userSrv;
            _sourceSrv = sourceSrv;
        }

        public IActionResult Index()
        {
            //var chemin = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "src", "ballon.glb");
            //if (!System.IO.File.Exists(chemin))
            //    return NotFound();

            //byte[] fichierData = System.IO.File.ReadAllBytes(chemin);
            //string fichierBase64 = Convert.ToBase64String(fichierData);


            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
