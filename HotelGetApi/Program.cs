using HotelGetApi;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<backgroundworkertest>();
builder.Services.AddScoped<IHotelDataService, HotelDataService>();

var connectionString = builder.Configuration.GetConnectionString("HotelGetterString");
builder.Services.AddDbContext<HotelDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowEvery", z=>z.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});

builder.Services.AddHttpClient();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowEvery");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
