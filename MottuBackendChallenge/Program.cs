using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//TODO: Ao final limpar codigo
// builder.Services.AddControllers(opts => {
//     opts.Filters.Add<ValidateModelStateAttribute>();
// });

// Add services to the container.
builder.Services.Configure<MongoSettings>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddSingleton<MongoDbContext>();

// Add Services and Repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<MotorcycleRepository>();
builder.Services.AddScoped<MotorcycleService>();

builder.Services.AddScoped<DeliverymanRepository>();
builder.Services.AddScoped<DeliverymanService>();

builder.Services.AddScoped<RentalPriceTableRepository>();

// Add Others Configurations
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

// Inicialização do contexto do mongodb, com criação de collections se necessário e inserção de valores
using (var sp = app.Services.CreateScope())
{
    var mongodbContext = sp.ServiceProvider.GetRequiredService<MongoDbContext>();

    await mongodbContext.Initialized();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
