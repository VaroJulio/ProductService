using Ardalis.ListStartupServices;
using FastEndpoints;
using FastEndpoints.ApiExplorer;
using FastEndpoints.Swagger.Swashbuckle;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductService.Api.Endpoints.ProductEndpoints.Create;
using ProductService.Infrastructure.Data;
using ProductService.Infrastructure.Extensions;
using ProductService.UseCases.Extensions;
using ProductService.UseCases.Mappers;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

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
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter the token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
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
            Array.Empty<string>()
        }
    });
    c.EnableAnnotations();
    c.OperationFilter<FastEndpointsOperationFilter>();
});

builder.Services
.AddAuthentication()
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorizationBuilder();
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
});

app.UseAuthorization();
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
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error ocurred migrating the DB. {error}", ex.Message);
}

var sepatator = new string('#', 59);
Console.WriteLine(sepatator);
Console.WriteLine($"### Starting {Assembly.GetExecutingAssembly().GetName().Name} in {builder.Environment.EnvironmentName}\t\t###");
Console.WriteLine(sepatator);

app.Run();
