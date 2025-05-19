using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using ArsalanApp.ActionFilters;
using ArsalanApp.DTOs;
using ArsalanApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArsalanApp.Controllers;

[Route("api/customIdentity")]
[ApiController]
public class CustomIdentityController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserStore<AppUser> _userStore;
    private readonly IUserEmailStore<AppUser> _emailStore;

    private static readonly EmailAddressAttribute _emailValidator = new();

    public CustomIdentityController(UserManager<AppUser> userManager,
                                    IUserStore<AppUser> userStore)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<AppUser>)userStore;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto registerDto)
    {
        if (string.IsNullOrEmpty(registerDto.Email) || !_emailValidator.IsValid(registerDto.Email))
        {
            return BadRequest(new { Message = "Invalid email format." });
        }

        AppUser user = new AppUser()
        {
            ContactNo = registerDto.ContactNo,
            DateOfBith = registerDto.DateOfBirth
        };

        await _userStore.SetUserNameAsync(user, registerDto.UserName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, registerDto.Email, CancellationToken.None);

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { Message = "User registered successfully." });
    }

    [HttpGet("BenchmarkTest")]
    [LogActionFilter]
    public async Task<IActionResult> BenchmarkTest()
    {
        Debug.WriteLine("----------------------In Controller");
        Console.WriteLine("----------------------In Controller");

        WeatherForecast weather = new();

        Random random = new Random();
        float delay = random.Next(100, 301);

        await weather.SomeTask(1);

        return Ok($"random delay is {delay / 1000} seconds");
    }

}
