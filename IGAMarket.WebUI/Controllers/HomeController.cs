using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IGAMarket.WebUI.Models;

namespace IGAMarket.WebUI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult IslemlerIndex()
    {
        return View();
    }  
    public IActionResult ClosingIndex()
    {
        return View();
    }
    public IActionResult SatisIndex()
    {
        return View();
    }



}
