using Microsoft.EntityFrameworkCore;
using WebApplicationApi.Data;
using WebApplicationApi.Endpoints;
using WebApplicationApi.Interface;
using WebApplicationApi.Services;

var builder = WebApplication.CreateBuilder(args);
var connections = builder.Configuration.GetConnectionString("DefaultConnection")!;
var uploadImageFolderPath = builder.Configuration.GetSection("UploadImageFolderPath").ToString()!;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

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
app.UseStaticFiles();

app.MapProductEndpoints();
app.MapDateTimeEndpoints();
app.MapUploadFileEndpoints();

app.Run();