using CompanyOrderManagement.Persistence;
using CompanyOrderManagement.Application;
using CompanyOrderManagement.Application.Validators.Companies;
using FluentValidation.AspNetCore;
using CompanyOrderManagement.Application.Features.Companies.Commands.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using CompanyOrderManagement.Application.ResponseModels;
using CompanyOrderManagement.Infrastructure.Filters;
using WebAPI.Middleware;
using CompanyOrderManagement.Persistence.Contexts;
using CompanyOrderManagement.Persistence.SeedingData;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using CompanyOrderManagement.Infrastructure;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.MSSqlServer;
using Serilog.Events;
using CompanyOrderManagement.Application.Logging.ConfigurationModels.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using CompanyOrderManagement.Domain.Entities.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfastructureServices();

builder.Services.Configure<LoggingConfiguration>(builder.Configuration.GetSection("SeriLogConfigurations"));
builder.Host.UseSerilog();

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
.AddFluentValidation(configuration =>
   configuration.RegisterValidatorsFromAssemblyContaining<UpdateCompanyCommandRequestValidator>())
.ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
.AddJsonOptions(options =>
 {
     options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
 });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Bearer token ile kimlik doðrulamasý yapýn"
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
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    
    .AddJwtBearer("Admin",options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CompanyOrderManagementDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();

    context.Database.Migrate();

    var seedData = new SeedData(context,userManager);
     seedData.Seeding();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
