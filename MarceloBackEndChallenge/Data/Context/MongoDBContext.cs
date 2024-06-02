using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

public class MongoDBContext : DbContext
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Deliveryman> DeliveryDrivers { get; set; }
    public DbSet<MotorcycleRental> MotorcycleRentals { get; set; }
    public DbSet<RequestRace> RequestRides { get; set; }

    private readonly IOptions<MongoSettings> _settings;

    public MongoDBContext(IOptions<MongoSettings> settings, DbContextOptions<MongoDBContext> options): base(options)
    {
        _settings = settings;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var mongoClientSettings = MongoClientSettings.FromConnectionString(_settings.Value.ConnectionString);
            var mongoClient         = new MongoClient(mongoClientSettings);

            optionsBuilder.UseMongoDB(mongoClient, _settings.Value.DatabaseName);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Motorcycle>().HasIndex(m => m.Plate).IsUnique();

        modelBuilder.Entity<Deliveryman>().HasIndex(m => m.CNH).IsUnique();

        modelBuilder.Entity<Deliveryman>().HasIndex(m => m.CNPJ).IsUnique();
    }
}