using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

builder.Services.AddDbContext<MongoDBContext>();

builder.Services.AddScoped<MotorcycleService>();
builder.Services.AddScoped<DeliverymanService>();
builder.Services.AddScoped<MotorcycleRentalService>();
builder.Services.AddScoped<RequestRaceService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Type = SecuritySchemeType.Http,
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
});

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    string secretKey = builder.Configuration["JWT:SecretKey"] ?? "";
    var key          = Encoding.ASCII.GetBytes(secretKey);

    options.RequireHttpsMetadata      = false;
    options.SaveToken                 = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(key),
        ValidateIssuer           = false,
        ValidateAudience         = false
    };

    options.Events = new JwtBearerEvents()
    {
        OnChallenge = context => 
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            // Escreva a mensagem de não autorizado no corpo da resposta.
            var message = new { error = "Unauthorized", message = "Você não tem permissão para acessar este recurso." };
            var json = JsonSerializer.Serialize(message);
            return context.Response.WriteAsync(json);
        }
    };
});

builder.Services.AddAuthorization();

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
