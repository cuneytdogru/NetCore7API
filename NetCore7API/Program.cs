using Microsoft.Extensions.Options;
using NetCore7API.EFCore;
using NetCore7API.Middlewares;
using NetCore7API.Services;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddEFCore(builder.Configuration)
    .AddServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AngularLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowCredentials();
        policy.AllowAnyMethod();
        policy.AllowAnyHeader();
    });
});

var app = builder.Build();

app.ConfigureCustomResponseWrapperMiddleware();
app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AngularLocalhost");

app.Run();