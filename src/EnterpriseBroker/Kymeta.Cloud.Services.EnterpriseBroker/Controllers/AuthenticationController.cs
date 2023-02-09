using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Kymeta.Cloud.Services.EnterpriseBroker.Controllers;

[Route("Auth")]
[ExcludeFromCodeCoverage]
public class AuthenticationController : Controller
{
    private ILogger<AuthenticationController> _logger;
    private IConfiguration _config;

    public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    [HttpGet("Login")]
    public IActionResult Login(string provider, string returnUrl) => Challenge(new AuthenticationProperties
    {
        RedirectUri = Url.Action("ExternalProviderLoginCallback"),
        Items =
            {
                { "scheme", provider },
                { "returnUrl", returnUrl }
            }
    }, "oidc");

    [HttpGet("Logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync("oidc");
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> ExternalProviderLoginCallback()
    {
        // Authenticate with extrenal provider
        var result = await HttpContext.AuthenticateAsync();
        // If fail, throw an exception and bail
        if (result?.Succeeded != true)
        {
            _logger.LogError("External authentication error.");
            throw new Exception("External authentication error.");
        }

        var returnUrl = result.Properties.Items["returnUrl"];

        // Retrieve User
        var externalUser = result.Principal;
        // Get the claims
        var externalUserClaims = externalUser.Claims.ToList();

        // Retrieve the userId from the external provider -- we will use it to look up the user in our database
        var userIdClaim = externalUserClaims.FirstOrDefault(x => x.Type == "sub");
        if (userIdClaim == null)
        {
            userIdClaim = externalUserClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        }
        // If we can't find an Id, throw, because there's something wrong with the profile coming back
        if (userIdClaim == null)
        {
            _logger.LogError("Couldn't retrieve user Id from external provider.");
            throw new Exception("Couldn't retrieve user Id from external provider.");
        }

        var userEmailAddress = externalUserClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

        // Ensure user is allowed to log in
        var allowedUsers = _config.GetValue<string>("Authentication:AllowedUsers").Split(',');
        var userName = userEmailAddress.Value.Split('@').First();
        var userIsAllowed = allowedUsers.Contains(userName);
        if (!userIsAllowed)
        {
            // Return with unauthorized since we aim to add users ourselves.
            await HttpContext.SignOutAsync();
            return Redirect("~/Unauthorized");
        }

        // Set up new claims array
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userIdClaim.Value),
            new Claim(ClaimTypes.Email, userEmailAddress.Value)
        };

        // Get the SessionId from the external provider, as we will be using it to sign out
        var sid = externalUserClaims.FirstOrDefault(x => x.Type == "sid");
        if (sid != null)
        {
            claims.Add(new Claim(ClaimTypes.Sid, sid.Value));
        }

        // Create the identity and principal
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.NameIdentifier, ClaimTypes.Role);
        var principal = new ClaimsPrincipal(identity);
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
            IsPersistent = true,
            IssuedUtc = DateTimeOffset.UtcNow
        };

        // Sign them in
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        // if we have a return url, use it
        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        // otherwise return to root
        return Redirect("~/");
    }
}

