using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;
using WebApplicationApi.Endpoints;
using WebApplicationApi.Interface;
using WebApplicationApi.Services;

var builder = WebApplication.CreateBuilder(args);
string connections = builder.Configuration.GetConnectionString("DefaultConnection")!;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connections));

builder.Services.AddTransient<ITimeService, DateTimeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapProductEndpoints();
app.MapDateTimeEndpoints();

app.Run();