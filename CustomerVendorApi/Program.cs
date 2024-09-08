using Carter;
using CustomerVendorApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

const string policyName = "CorsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policyName, builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var jwtSettings = builder.Configuration.GetSection("JWT");

builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5000/");
});

builder.Services.AddDbContext<ProductsServicesDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CustomerVendorConnection"));
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
builder.Services.AddCarter();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = jwtSettings["ValidAudience"],
                ValidIssuer = jwtSettings["ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
            };
        });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

app.UseCors(policyName);


app.MapCarter();

app.Run();