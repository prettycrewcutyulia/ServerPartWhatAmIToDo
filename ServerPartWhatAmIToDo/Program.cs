using Microsoft.OpenApi.Models;

// Создаете WebApplicationBuilder
var builder = WebApplication.CreateBuilder(args);

// Добавляете поддержку контроллеров
builder.Services.AddControllers();

// Конфигурируете Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "My API",
        Description = "A simple example ASP.NET Core Web API"
    });
});

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

app.UseAuthorization();

// Маппинг эндпоинтов Health Checks
app.MapHealthChecks("/health");

// Маппинг контроллеров
app.MapControllers();

// Запускаете приложение
app.Run();