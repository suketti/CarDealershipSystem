using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Services.Location.Context;
using Services.Location.Mappings;
using Services.Location.Services;

namespace Services.Location;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        //connectionString = connectionString.Replace("{dbpass}", dbPassword);
        // Add services to the container.

        builder.Services.AddAutoMapper(typeof(MappingProfile));
        builder.Services.AddScoped<LocationService>();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //TODO: When dockerized, make the password an ENV variable. Do not store it in the connection string. (Left like that for DEV purposes as of now)
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}