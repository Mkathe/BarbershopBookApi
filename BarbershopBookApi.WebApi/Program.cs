using System.Text;
using BarbershopBookApi.Application.Interfaces;
using BarbershopBookApi.Application.Services;
using BarbershopBookApi.Infrastructure;
using BarbershopBookApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using ILogger = Serilog.ILogger;

namespace BarbershopBookApi;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .CreateLogger();
        var builder = WebApplication.CreateBuilder(args);
        //Host
        builder.Host.UseSerilog();
        // Add services to the container.
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Token"])),
                ValidateIssuerSigningKey = true
            };
        });
        builder.Services.AddAuthorization();
        builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "BookingApi",
                    Version = "v1",
                    Description = "BookingApi Swagger",
                });
                var security = new OpenApiSecurityScheme()
                {
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Jwt authentication",
                    Type = SecuritySchemeType.Http,
                    In = ParameterLocation.Header,
                    Reference = new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                options.AddSecurityDefinition(security.Reference.Id, security);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        security,
                        []
                    }
                });
            });
        //Injections
        builder.Services.AddScoped<IApplicationDbContext>(options => 
            options.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IHairdresserRepository, HairdresserRepository>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IBookingService, BookingService>();
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();
        app.Run();
    }
}