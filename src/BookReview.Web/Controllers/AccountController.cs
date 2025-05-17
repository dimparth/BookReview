using BookReview.ApplicationCore.Interfaces;
using BookReview.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookReview.Web.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var succeeded = await _userService.RegisterAsync(model.Username, model.Email, model.Password);
        if (succeeded)
            return RedirectToAction("Index", "Home");

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var success = await _userService.LoginAsync(model.Username, model.Password, model.RememberMe);
        if (success)
            return RedirectToAction("Index", "Home");

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _userService.LogoutAsync();
        return RedirectToAction("Index", "Home");
    }
}