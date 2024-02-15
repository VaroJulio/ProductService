using Ardalis.ListStartupServices;
using FastEndpoints;
using FastEndpoints.ApiExplorer;
using FastEndpoints.Swagger.Swashbuckle;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductService.Api.Endpoints.ProductEndpoints.Create;
using ProductService.Api.Processors;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Extensions;
using ProductService.UseCases.Extensions;
using ProductService.UseCases.Mappers;
using Serilog;
using Serilog.Filters;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, services, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
    config.WriteTo.Logger(c => c.Filter.ByIncludingOnly(Matching.FromSource<LogDurationWatcherPostProcessor>())
        .WriteTo.File(".logs/logRequestDuration.txt", rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 268435456, shared: true));
}, writeToProviders: false);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
const string corsPolicyName = "CorsPolicy";

builder.Services.AddInfrastructureModule();
builder.Services.AddUseCasesModule();

builder.Services.AddDbContext<AppDbContext>((sp, optionsBuilder) =>
{
    optionsBuilder.UseSqlServer(connectionString);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(UseCasesProfileMapper), typeof(CreateProductMapper));
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policyBuilder =>
    {
        policyBuilder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("content-disposition")
            .WithOrigins("http://localhost:8080");
    });
});

builder.Services.AddControllers();
builder.Services.AddFastEndpoints();
builder.Services.AddFastEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = Assembly.GetExecutingAssembly().GetName().Name, Version = "v1" });
    c.EnableAnnotations();
    c.OperationFilter<FastEndpointsOperationFilter>();
});

builder.Services.Configure<ServiceConfig>(config =>
{
    config.Services = new List<ServiceDescriptor>(builder.Services);
    config.Path = "/servicelist";
});

var app = builder.Build();
app.UseDefaultExceptionHandler(logStructuredException: true);

if (app.Environment.IsProduction())
    app.UseHttpsRedirection();
else
{
    app.UseShowAllServicesMiddleware();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Assembly.GetExecutingAssembly().GetName().Name} v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors(corsPolicyName);
app.UseRouting();

app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Versioning.Prefix = "version";
    c.Endpoints.Configurator = ep =>
    {
        ep.PreProcessor<InitDurationWatcherPreprocessor>(Order.Before);
        ep.PostProcessor<LogDurationWatcherPostProcessor>(Order.After);
    };
});

app.MapDefaultControllerRoute();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetService<AppDbContext>();
    Console.WriteLine($"### Database connection: {context!.Database.GetConnectionString()}\t\t###");
    context.Database.Migrate();
}
catch (Exception ex)
{
    var loggerLog = services.GetRequiredService<ILogger<Program>>();
    loggerLog.LogError(ex, "An error ocurred migrating the DB. {error}", ex.Message);
}

var sepatator = new string('#', 59);
Console.WriteLine(sepatator);
Console.WriteLine($"### Starting {Assembly.GetExecutingAssembly().GetName().Name} in {builder.Environment.EnvironmentName}\t\t###");
Console.WriteLine(sepatator);

app.Run();
