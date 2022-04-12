using ApiTodoBackend.Data;
using ApiTodoBackend.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", builder =>
    {
        builder.WithOrigins("https://todobackend.com")
            .WithMethods("GET", "PATCH", "POST", "DELETE")
            .WithHeaders("Content-Type");
    });

    options.AddPolicy("Development", builder =>
    {
        builder.AllowAnyOrigin()
            .WithMethods("GET", "PATCH", "POST", "DELETE")
            .WithHeaders("Content-Type");
    });
});

builder.Services.AddScoped<ITodoRepository, InMemoryTodoRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


WebApplication app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpLogging();
}

app.UseCors(app.Configuration["Cors:DefaultPolicy"]);
app.MapControllers();

app.Run();
