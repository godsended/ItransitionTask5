using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ItransitionTask5.Models;
using ItransitionTask5.Services;
using ItransitionTask5.ViewModels;

namespace ItransitionTask5.Controllers;

public class HomeController : Controller
{
    private readonly IHostInfo hostInfo;
    private readonly NamesContext namesContext;

    public HomeController(IHostInfo hostInfo, NamesContext namesContext)
    {
        this.hostInfo = hostInfo;
        this.namesContext = namesContext;
    }

    public IActionResult Index()
    {
        if (!Request.Cookies.ContainsKey("Name"))
        {
            Debug.WriteLine("Redirect to login");
            return RedirectToAction("Index", "Login");
        }
        return View(new HomeViewModel() {Name = Request.Cookies["Name"]!, HostUrl = hostInfo.GetHostUrl(),
            NamesContext = namesContext
        });
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}