using Microsoft.EntityFrameworkCore;
using SovComTestApp.Data;
using SovComTestApp.Services;
using SovComTestApp.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IInvitationService, InvitationService>();
builder.Services.AddTransient<IValidators, Validators>();

var connectionsString = builder.Configuration.GetConnectionString("InvitationDbContext");
builder.Services.AddDbContext<InvitationDbContext>(options => { options.UseSqlServer(connectionsString); },
    ServiceLifetime.Transient);

var app = builder.Build();

await using var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<InvitationDbContext>();
context.Database.EnsureCreated();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
