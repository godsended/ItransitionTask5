using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ItransitionTask5.Controllers;

public class LoginController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        if (CheckNameCookie())
            return RedirectToAction("Index", "Home");
        return View();
    }

    [HttpGet]
    public IActionResult ChangeName()
    {
        ClearCookies();
        return RedirectToAction("Index", "Login");
    }

    [HttpPost]
    public IActionResult Login()
    {
        string name = Request.Form["Name"];
        if (!string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name))
        {
            ConfigureCookies(name);
            return RedirectToAction("Index", "Home");
        }

        return Index();
    }

    private bool CheckNameCookie()
    {
        return Request.Cookies.ContainsKey("Name");
    }

    private void ClearCookies()
    {
        Response.Cookies.Delete("Name");
    }

    private void ConfigureCookies(string name)
    {
        ClearCookies();
        DateTime expires = DateTime.Now.AddYears(1);
        CookieOptions options = new CookieOptions();
        options.Expires = expires;
        Response.Cookies.Append("Name", name, options);
    }
}