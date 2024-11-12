using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Title = "Dashboard";
        return View();
    }
}