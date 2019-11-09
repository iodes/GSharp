using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GSharp.Studio.Models;
using ElectronNET.API;
using System.Linq;

namespace GSharp.Studio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            Electron.IpcMain.On("async-msg", data =>
            {
                var mainWindow = Electron.WindowManager.BrowserWindows.First();
                Electron.IpcMain.Send(mainWindow, "asynchronous-reply", "Success!");
            });

            return View();
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
