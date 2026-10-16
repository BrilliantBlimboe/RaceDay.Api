using Microsoft.AspNetCore.Mvc;
namespace RaceDay.Api.Controllers;
[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
 protected int? CurrentUserId => HttpContext.Session.GetInt32("UserID");
 protected string? CurrentRole => HttpContext.Session.GetString("Role");
 protected IActionResult? RequireLogin() => CurrentUserId is null ? Unauthorized("Log in first.") : null;
 protected IActionResult? RequireRole(string role) => CurrentUserId is null ? Unauthorized("Log in first.") : CurrentRole != role ? Forbid() : null;
}
