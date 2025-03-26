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
        builder.Services.AddScoped<IUserService, UserService>(); 
        builder.Services.AddScoped(provider => new Lazy<IUserService>(provider.GetRequiredService<IUserService>));
        builder.Services.AddScoped<JWTService>();
        builder.Services.AddScoped<RoleService>();
        builder.Services.AddScoped<CarService>();
        builder.Services.AddScoped<CarMetadataService>();
        builder.Services.AddScoped<CarMakerService>();
        builder.Services.AddScoped<CarModelService>();
        builder.Services.AddScoped<EngineSizeService>();
        builder.Services.AddScoped<ImageService>();
        builder.Services.AddScoped<IMessageService, MessageService>();
        builder.Services.AddScoped<ISavedCarService, SavedCarService>();
        builder.Services.AddScoped<IReservationService, ReservationService>();
        
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
        
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy.WithOrigins("http://localhost:5173") 
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

        builder.Services.AddResponseCompression();
        
        var webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        if (!Directory.Exists(webRootPath))
        {
            Directory.CreateDirectory(webRootPath);
        }

        builder.Environment.WebRootPath = webRootPath; 
        
        var app = builder.Build();
        
        app.UseResponseCompression();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // app.Use(async (context, next) =>
        // {
        //     if (context.Request.Method == "OPTIONS")
        //     {
        //         context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        //         context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
        //         context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
        //         context.Response.StatusCode = 204; // No Content
        //         await context.Response.CompleteAsync();
        //         return;
        //     }
        //     await next();
        // });

        
        app.UseCors("AllowSpecificOrigin");
        
        app.UseHttpsRedirection();

        
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseStaticFiles();
        
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await SeedRoles.Initialize(services);
        }
        
        await app.RunAsync();
    }
}