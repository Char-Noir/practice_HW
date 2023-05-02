using System.Diagnostics;
using HW4.DTO;
using Microsoft.AspNetCore.Mvc;
using HW4.Models;
using HW4.Services;

namespace HW4.Controllers;

public class RegistrController : Controller
{
    private readonly ILogger<RegistrController> _logger;
    private readonly IUserService _userService;

    public RegistrController(ILogger<RegistrController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    
    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Result(int? id)
    {
        if (id == null)
        {
            ViewBag.Error = true;
            ViewBag.ErrorMessage = "User not found";
            return View();
        }
        var result = await _userService.GetUserAsync(id.Value);
        ViewBag.User = result;
        if (!result.Success)
        {
            ViewBag.Error = true;
            ViewBag.ErrorMessage = result.Error;
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
    
    [HttpPost]
    public async Task<IActionResult> Index( UserRequestDto user)
    {
        if (user == null || user.UserEmail == null || user.UserName == null)
        {
            ViewBag.Error = true;
            ViewBag.ErrorMessage = "Fill an empty fields.";
            return View(user);
        }
        var result =  await _userService.AddUserAsync(user);
        if (!result.Success)
        {
            ViewBag.Error = true;
            ViewBag.ErrorMessage = result.Error;
            return View( user);
        }
        
        return RedirectToAction("Result", "Registr", new RouteValueDictionary { { "id", result.Id } });
    }
}