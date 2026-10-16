using Microsoft.AspNetCore.Identity; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore; using RaceDay.Api.Data; using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[Route("api/users")]
public class UsersController(RaceDayContext db, IPasswordHasher<User> hasher) : ApiControllerBase
{
 public record UpdateProfileDto(string FirstName,string LastName,string Email,string? Phone,DateTime? DateOfBirth);
 [HttpGet("me")] public async Task<IActionResult> Me(){if(RequireLogin() is IActionResult e)return e;var u=await db.Users.FindAsync(CurrentUserId);return u is null?NotFound():Ok(new{u.UserID,u.FirstName,u.LastName,u.Email,u.Phone,u.DateOfBirth,u.Role,u.CreatedAt});}
 [HttpPut("me")] public async Task<IActionResult> Update(UpdateProfileDto d){if(RequireLogin() is IActionResult e)return e;if(string.IsNullOrWhiteSpace(d.FirstName)||string.IsNullOrWhiteSpace(d.LastName)||string.IsNullOrWhiteSpace(d.Email))return BadRequest("Name and email are required.");var u=await db.Users.FindAsync(CurrentUserId);if(u is null)return NotFound();var email=d.Email.Trim().ToLower();if(await db.Users.AnyAsync(x=>x.Email==email&&x.UserID!=u.UserID))return Conflict("Email already in use.");u.FirstName=d.FirstName.Trim();u.LastName=d.LastName.Trim();u.Email=email;u.Phone=d.Phone;u.DateOfBirth=d.DateOfBirth;await db.SaveChangesAsync();return Ok(new{u.UserID,u.FirstName,u.LastName,u.Email,u.Phone,u.DateOfBirth,u.Role});}
}
