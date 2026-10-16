using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => c.SwaggerDoc("v1", new() { Title = "RaceDay API", Version = "v1", Description = "RaceDay API. Login first; the session cookie is used for subsequent requests." }));
builder.Services.AddDbContext<RaceDayContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("RaceDay")));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => { o.Cookie.Name = ".RaceDay.Session"; o.Cookie.HttpOnly = true; o.Cookie.IsEssential = true; o.IdleTimeout = TimeSpan.FromMinutes(30); });
builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<RaceDay.Api.Models.User>, Microsoft.AspNetCore.Identity.PasswordHasher<RaceDay.Api.Models.User>>();
var app = builder.Build();
if (app.Environment.IsDevelopment()) { app.UseSwagger(); app.UseSwaggerUI(); }
app.UseHttpsRedirection();
app.UseSession();
app.MapControllers();
app.Run();
public partial class Program { }
