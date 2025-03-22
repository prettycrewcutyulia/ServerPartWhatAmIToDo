using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ServerPartWhatAmIToDo;

// Создаете WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Environment.GetEnvironmentVariable("ValidIssuer"),
            ValidAudience = Environment.GetEnvironmentVariable("ValidAudience"),
            IssuerSigningKey = 
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRETKEY")))
        };
    });

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

// Конфигурируете Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "My API",
        Description = "A simple example ASP.NET Core Web API"
    });
    
    // Определяем схемы безопасности
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Введите 'Bearer' [пробел] и ваш токен",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Добавляете поддержку контроллеров
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

// Это ваша регистрация сервисов внутри метода расширения
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddServices();

// Добавление Health Checks
builder.Services.AddHealthChecks();

// Строите приложение
var app = builder.Build();

// Включаете и настраиваете Swagger/SwaggerUI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;  // Позволяет открывать Swagger UI на корневом URL
    });
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Маппинг эндпоинтов Health Checks
app.MapHealthChecks("/health");

// Маппинг контроллеров
app.MapControllers();

// Запускаете приложение
app.Run();