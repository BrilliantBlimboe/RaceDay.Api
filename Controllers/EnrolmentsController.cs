using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers;

[Route("api")]
public class EnrolmentsController(RaceDayContext db) : ApiControllerBase
{
    public record EnrolDto(int CategoryId);

    [HttpPost("events/{eventId:int}/enrolments")]
    public async Task<IActionResult> Enrol(int eventId, EnrolDto d)
    {
        if (RequireRole("Participant") is IActionResult e) return e;

        if (!await db.Events.AnyAsync(x => x.EventID == eventId))
            return NotFound("Event not found.");

        if (!await db.Categories.AnyAsync(x => x.CategoryID == d.CategoryId && x.EventID == eventId))
            return NotFound("Category not found for this event.");

        if (await db.Registrations.AnyAsync(x => x.EventID == eventId && x.UserID == CurrentUserId))
            return Conflict("Already enrolled in this event.");

        var r = new Registration
        {
            UserID = CurrentUserId!.Value,
            EventID = eventId,
            CategoryID = d.CategoryId,
            RegistrationDate = DateTime.Now,
            Status = "Active", // FIXED: DB only allows DNS, Cancelled, Active
            PaymentStatus = "Pending"
        };

        db.Registrations.Add(r);
        await db.SaveChangesAsync();

        return StatusCode(201, new { r.RegistrationID, r.EventID, r.CategoryID, r.RegistrationDate, r.Status });
    }

    [HttpGet("enrolments/me")]
    public async Task<IActionResult> Mine()
    {
        if (RequireRole("Participant") is IActionResult e) return e;

        var rows = await (from r in db.Registrations
                          join ev in db.Events on r.EventID equals ev.EventID
                          join c in db.Categories on r.CategoryID equals c.CategoryID
                          where r.UserID == CurrentUserId
                          select new
                          {
                              r.RegistrationID,
                              r.EventID,
                              EventName = ev.EventName,
                              Category = c.CategoryName,
                              r.RegistrationDate,
                              r.PaymentStatus,
                              r.Status
                          }).ToListAsync();
        return Ok(rows);
    }

    [HttpGet("events/{eventId:int}/enrolments")]
    public async Task<IActionResult> ForEvent(int eventId)
    {
        if (RequireRole("Organiser") is IActionResult e) return e;

        if (!await db.Events.AnyAsync(x => x.EventID == eventId))
            return NotFound();

        if (!await db.EventOrganisers.AnyAsync(x => x.EventID == eventId && x.UserID == CurrentUserId))
            return Forbid();

        var rows = await (from r in db.Registrations
                          join u in db.Users on r.UserID equals u.UserID
                          join c in db.Categories on r.CategoryID equals c.CategoryID
                          where r.EventID == eventId
                          select new
                          {
                              r.RegistrationID,
                              r.EventID,
                              Participant = u.FirstName + " " + u.LastName,
                              u.Email,
                              Category = c.CategoryName,
                              r.RegistrationDate,
                              r.Status
                          }).ToListAsync();
        return Ok(rows);
    }
}