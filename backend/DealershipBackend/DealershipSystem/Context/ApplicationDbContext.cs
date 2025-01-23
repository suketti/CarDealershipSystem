using System.Configuration;
using DealershipSystem.Configurations;
using DealershipSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DealershipSystem.Context;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Models.Location> Locations { get; set; }
    public DbSet<Models.Prefecture> Prefectures { get; set; }
    public DbSet<Models.Address> Addresses { get; set; }
    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<CarMaker> CarMakers { get; set; } = null!;
    public DbSet<CarModel> CarModels { get; set; } = null!;
    public DbSet<BodyType> BodyTypes { get; set; } = null!;
    public DbSet<EngineSizeModel> EngineSizeModels { get; set; } = null!;
    public DbSet<FuelType> FuelTypes { get; set; } = null!;
    public DbSet<DrivetrainType> DrivetrainTypes { get; set; } = null!;
    public DbSet<TransmissionType> TransmissionTypes { get; set; } = null!;
    public DbSet<Color> Colors { get; set; } = null!;
    public DbSet<CarExtra> CarExtras { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PrefectureConfiguration());
        
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(e => e.UserId);  

        modelBuilder.Entity<IdentityUserRole<string>>()
            .HasKey(e => new { e.UserId, e.RoleId }); 

        modelBuilder.Entity<IdentityUserClaim<string>>()
            .HasKey(e => e.Id);  

        modelBuilder.Entity<IdentityUserToken<string>>()
            .HasKey(e => new { e.UserId, e.LoginProvider, e.Name });
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Car>()
            .HasMany(c => c.Images)
            .WithOne(i => i.Car)
            .HasForeignKey(i => i.CarID);

        modelBuilder.Entity<Car>()
            .HasMany(c => c.CarExtras)
            .WithOne(ce => ce.Car)
            .HasForeignKey(ce => ce.CarID);

        modelBuilder.Entity<CarModel>()
            .HasMany(m => m.EngineSizes)
            .WithOne(es => es.CarModel)
            .HasForeignKey(es => es.ModelID);
        
        modelBuilder.Entity<EngineSizeModel>()
            .HasOne(es => es.CarModel)
            .WithMany(m => m.EngineSizes)
            .HasForeignKey(es => es.ModelID)
            .OnDelete(DeleteBehavior.Cascade);

        // Car and EngineSizeModel relationship
        modelBuilder.Entity<Car>()
            .HasOne(c => c.EngineSize)
            .WithMany()
            .HasForeignKey(c => c.EngineSizeID)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Prefecture>().HasData(
            new Prefecture { Id = 1, Name = "Hokkaido", NameJP = "北海道" },
            new Prefecture { Id = 2, Name = "Aomori", NameJP = "青森県" },
            new Prefecture { Id = 3, Name = "Iwate", NameJP = "岩手県" },
            new Prefecture { Id = 4, Name = "Miyagi", NameJP = "宮城県" },
            new Prefecture { Id = 5, Name = "Akita", NameJP = "秋田県" },
            new Prefecture { Id = 6, Name = "Yamagata", NameJP = "山形県" },
            new Prefecture { Id = 7, Name = "Fukushima", NameJP = "福島県" },
            new Prefecture { Id = 8, Name = "Ibaraki", NameJP = "茨城県" },
            new Prefecture { Id = 9, Name = "Tochigi", NameJP = "栃木県" },
            new Prefecture { Id = 10, Name = "Gunma", NameJP = "群馬県" },
            new Prefecture { Id = 11, Name = "Saitama", NameJP = "埼玉県" },
            new Prefecture { Id = 12, Name = "Chiba", NameJP = "千葉県" },
            new Prefecture { Id = 13, Name = "Tokyo", NameJP = "東京都" },
            new Prefecture { Id = 14, Name = "Kanagawa", NameJP = "神奈川県" },
            new Prefecture { Id = 15, Name = "Niigata", NameJP = "新潟県" },
            new Prefecture { Id = 16, Name = "Toyama", NameJP = "富山県" },
            new Prefecture { Id = 17, Name = "Ishikawa", NameJP = "石川県" },
            new Prefecture { Id = 18, Name = "Fukui", NameJP = "福井県" },
            new Prefecture { Id = 19, Name = "Yamanashi", NameJP = "山梨県" },
            new Prefecture { Id = 20, Name = "Nagano", NameJP = "長野県" },
            new Prefecture { Id = 21, Name = "Gifu", NameJP = "岐阜県" },
            new Prefecture { Id = 22, Name = "Shizuoka", NameJP = "静岡県" },
            new Prefecture { Id = 23, Name = "Aichi", NameJP = "愛知県" },
            new Prefecture { Id = 24, Name = "Mie", NameJP = "三重県" },
            new Prefecture { Id = 25, Name = "Shiga", NameJP = "滋賀県" },
            new Prefecture { Id = 26, Name = "Kyoto", NameJP = "京都府" },
            new Prefecture { Id = 27, Name = "Osaka", NameJP = "大阪府" },
            new Prefecture { Id = 28, Name = "Hyogo", NameJP = "兵庫県" },
            new Prefecture { Id = 29, Name = "Nara", NameJP = "奈良県" },
            new Prefecture { Id = 30, Name = "Wakayama", NameJP = "和歌山県" },
            new Prefecture { Id = 31, Name = "Tottori", NameJP = "鳥取県" },
            new Prefecture { Id = 32, Name = "Shimane", NameJP = "島根県" },
            new Prefecture { Id = 33, Name = "Okayama", NameJP = "岡山県" },
            new Prefecture { Id = 34, Name = "Hiroshima", NameJP = "広島県" },
            new Prefecture { Id = 35, Name = "Yamaguchi", NameJP = "山口県" },
            new Prefecture { Id = 36, Name = "Tokushima", NameJP = "徳島県" },
            new Prefecture { Id = 37, Name = "Kagawa", NameJP = "香川県" },
            new Prefecture { Id = 38, Name = "Ehime", NameJP = "愛媛県" },
            new Prefecture { Id = 39, Name = "Kochi", NameJP = "高知県" },
            new Prefecture { Id = 40, Name = "Fukuoka", NameJP = "福岡県" },
            new Prefecture { Id = 41, Name = "Saga", NameJP = "佐賀県" },
            new Prefecture { Id = 42, Name = "Nagasaki", NameJP = "長崎県" },
            new Prefecture { Id = 43, Name = "Kumamoto", NameJP = "熊本県" },
            new Prefecture { Id = 44, Name = "Oita", NameJP = "大分県" },
            new Prefecture { Id = 45, Name = "Miyazaki", NameJP = "宮崎県" },
            new Prefecture { Id = 46, Name = "Kagoshima", NameJP = "鹿児島県" },
            new Prefecture { Id = 47, Name = "Okinawa", NameJP = "沖縄県" }
        );
        
        modelBuilder.Entity<BodyType>().HasData(
            new BodyType { ID = 1, NameJapanese = "軽自動車", NameEnglish = "Kei Car" },
            new BodyType { ID = 2, NameJapanese = "コンパクトカー", NameEnglish = "Compact Car" },
            new BodyType { ID = 3, NameJapanese = "ミニバン/ワンボックス", NameEnglish = "Minivan/One-Box" },
            new BodyType { ID = 4, NameJapanese = "セダン", NameEnglish = "Sedan" },
            new BodyType { ID = 5, NameJapanese = "クーペ", NameEnglish = "Coupe" },
            new BodyType { ID = 6, NameJapanese = "ステーションワゴン", NameEnglish = "Station Wagon" },
            new BodyType { ID = 7, NameJapanese = "SUV/クロカン", NameEnglish = "SUV/Crossover" },
            new BodyType { ID = 8, NameJapanese = "トラック/バン", NameEnglish = "Truck/Van" }
        );
        
        modelBuilder.Entity<TransmissionType>().HasData(
            new TransmissionType { ID = 1, Type = "MT" },  // Manual Transmission
            new TransmissionType { ID = 2, Type = "AT" },  // Automatic Transmission
            new TransmissionType { ID = 3, Type = "CVT" }, // Continuously Variable Transmission
            new TransmissionType { ID = 4, Type = "AMT" }, // Automated Manual Transmission
            new TransmissionType { ID = 5, Type = "DCT" }  // Dual Clutch Transmission
        );
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}

