using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore7API.Authorization;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Services;
using NetCore7API.EFCore;
using NetCore7API.Middlewares;
using NetCore7API.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
NetCore7API.Domain.Models.AppSettings appSettings = new NetCore7API.Domain.Models.AppSettings();
configuration.Bind(appSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = AppSettings.Authentication.Issuer,
        ValidAudience = AppSettings.Authentication.Audience,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(AppSettings.Authentication.Key)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true
    };
});

// Add services to the container.

builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

builder.Services
    .AddHttpContextAccessor()
    .AddScoped<ITokenService, TokenService>()
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AngularLocalhost");

app.Run();