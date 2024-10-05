using Hangfire;
using HangFireApp.Context;
using HangFireApp.Controllers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionstring = builder.Configuration.GetConnectionString("SqlServer");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionstring);
});

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionstring);
});

builder.Services.AddHangfireServer();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();

RecurringJob.AddOrUpdate("test-job", () => BackgroundTestServices.Test(), Cron.Minutely());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
