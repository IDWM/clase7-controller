using System.Reflection;
using clase7_controller.Data;
using clase7_controller.DTOs;
using clase7_controller.Mappings;
using clase7_controller.Middleware;
using clase7_controller.Models;
using clase7_controller.Repositories.Implementations;
using clase7_controller.Repositories.Interfaces;
using clase7_controller.Services.Implementations;
using clase7_controller.Services.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StackExchange.Profiling.Storage;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        Dictionary<string, string[]> modelErrors = context
            .ModelState.Where(entry => entry.Value?.Errors.Count > 0)
            .ToDictionary(
                entry => entry.Key,
                entry => entry.Value?.Errors.Select(error => error.ErrorMessage).ToArray() ?? []
            );

        ApiErrorDataDto errorData = new()
        {
            ErrorCode = "VALIDATION_ERROR",
            TraceId = context.HttpContext.TraceIdentifier,
            Errors = modelErrors,
        };
        ApiResponse<ApiErrorDataDto> response = new()
        {
            Message = "Se encontraron errores de validación.",
            Data = errorData,
        };

        BadRequestObjectResult badRequestObjectResult = new(response);
        badRequestObjectResult.ContentTypes.Add("application/json");
        return badRequestObjectResult;
    };
});

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    options.Storage = new MemoryCacheStorage(
        new MemoryCache(new MemoryCacheOptions()),
        TimeSpan.FromMinutes(60)
    );
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

MapsterConfig.RegisterMappings();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    IValidator<Product> productValidator = scope.ServiceProvider.GetRequiredService<
        IValidator<Product>
    >();
    context.Database.Migrate();
    await DatabaseSeeder.SeedAsync(context, productValidator);
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiniProfiler();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
