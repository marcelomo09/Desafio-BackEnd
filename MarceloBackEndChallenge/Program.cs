using System.Net;
using System.Net.Mail;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDb"));

builder.Services.AddDbContext<MongoDBContext>();

builder.Services.AddScoped<MotorcycleService>();
builder.Services.AddScoped<DeliverymanService>();
builder.Services.AddScoped<MotorcycleRentalService>();
builder.Services.AddScoped<RequestRaceService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var context         = serviceProvider.GetRequiredService<MongoDBContext>();

    if (!context.Users.Any(u => u.UserName == "admin"))
    {
        var user = new User() { UserName = "admin", Password = builder.Configuration.GetSection("Admin").Value ?? "" };

        context.Users.Add(user);

        await context.SaveChangesAsync();
    }
}

app.Run();
