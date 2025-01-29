using System.Text;
using DealershipSystem.Context;
using DealershipSystem.Helpers;
using DealershipSystem.Interfaces;
using DealershipSystem.Mappings;
using DealershipSystem.Models;
using DealershipSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DealershipSystem;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<LocationService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<JWTService>(); 
        builder.Services.AddScoped<RoleService>();
        builder.Services.AddScoped<CarService>();
        builder.Services.AddScoped<CarMetadataService>();
        builder.Services.AddScoped<CarMakerService>();
        
        builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        
        builder.Services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        builder.Services.AddAuthorization();
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

        var app = builder.Build();

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
        
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedRoles.Initialize(services);
        }
        
        await app.RunAsync();
    }
}