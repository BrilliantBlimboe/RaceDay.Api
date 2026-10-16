using Microsoft.AspNetCore.Mvc; using Microsoft.AspNetCore.Identity; using Microsoft.EntityFrameworkCore; using RaceDay.Api.Data; using RaceDay.Api.Models;
namespace RaceDay.Api.Controllers;
[ApiController, Route("api/auth")]
public class AuthController(RaceDayContext db, IPasswordHasher<User> hasher) : ControllerBase
{
 public record RegisterDto(string FirstName,string LastName,string Email,string Password,string Role,string? Phone=null,DateTime? DateOfBirth=null);
 public record LoginDto(string Email,string Password);
 [HttpPost("register"), ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status400BadRequest)]
 public async Task<IActionResult> Register(RegisterDto d) { if(string.IsNullOrWhiteSpace(d.FirstName)||string.IsNullOrWhiteSpace(d.LastName)||string.IsNullOrWhiteSpace(d.Email)||d.Password.Length<8||!d.Password.Any(char.IsUpper)||!d.Password.Any(char.IsDigit)||!d.Password.Any(c=>!char.IsLetterOrDigit(c))||!(d.Role is "Participant" or "Organiser")) return BadRequest("Provide names, email, a strong password (8+ chars, uppercase, number and symbol), and Participant or Organiser role."); var email=d.Email.Trim().ToLowerInvariant(); if(await db.Users.AnyAsync(x=>x.Email==email)) return Conflict("Email already registered."); var u=new User{FirstName=d.FirstName.Trim(),LastName=d.LastName.Trim(),Email=email,Role=d.Role,Phone=d.Phone,DateOfBirth=d.DateOfBirth,CreatedAt=DateTime.UtcNow}; u.PasswordHash=hasher.HashPassword(u,d.Password); db.Users.Add(u); await db.SaveChangesAsync(); return Created("/api/users/me",new{u.UserID,u.FirstName,u.LastName,u.Email,u.Role}); }
 [HttpPost("login")]
 public async Task<IActionResult> Login(LoginDto d) { var u=await db.Users.SingleOrDefaultAsync(x=>x.Email==d.Email.Trim().ToLower()); if(u is null||hasher.VerifyHashedPassword(u,u.PasswordHash,d.Password)==PasswordVerificationResult.Failed) return Unauthorized("Invalid email or password."); HttpContext.Session.SetInt32("UserID",u.UserID); HttpContext.Session.SetString("Role",u.Role); return Ok(new{message="Login successful. Session cookie established.",user=new{u.UserID,u.FirstName,u.LastName,u.Email,u.Role}}); }
 [HttpPost("logout")] public IActionResult Logout(){HttpContext.Session.Clear();return NoContent();}
}
